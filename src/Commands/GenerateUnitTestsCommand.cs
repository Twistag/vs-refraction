using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.GenerateUnitTests)]
    internal sealed class GenerateUnitTestsCommand : BaseCommand<GenerateUnitTestsCommand>
    {
        private System.IServiceProvider _serviceProvider;
        private static List<LanguageProps> languages;

        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (_serviceProvider == null) {
                _serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.OLE.Interop.IServiceProvider)));
            }

            if (languages == null)
            {
                languages = await RefractionBackendService.getLanguages();
            }

            string language = VSServices.GetLanguage();
            LanguageProps foundLanguage = languages.Find(langConfig => langConfig.value == language);

            if (foundLanguage == null || foundLanguage.frameworks == null || foundLanguage.frameworks.Count == 0)
            {
                NotificationService.ShowErrorMessage("Language not supported." + language);
                return;
            }

            IEnumerable<string> labels = foundLanguage.frameworks.Select(frameWork => frameWork.label);
            string result = ShowLanguageSelectionWIndow(labels);
            if (result != null)
            {
                RefractionBackendService.GenerateAsync("unit-tests", foundLanguage.frameworks.Find(frameWork => frameWork.label == result).value);
            }
        }

        public string ShowLanguageSelectionWIndow(IEnumerable<string> labels)
        {
            IVsUIShell uiShell = (IVsUIShell)_serviceProvider.GetService(typeof(SVsUIShell));

            if (uiShell != null)
            {
                SelectionWindow selectionWindow = new SelectionWindow(labels);

                IntPtr hwnd;
                uiShell.GetDialogOwnerHwnd(out hwnd);

                WindowWrapper windowWrapper = new WindowWrapper(hwnd);
               
                DialogResult dialogResult = selectionWindow.ShowDialog(windowWrapper);
                if (DialogResult.OK == dialogResult)
                {
                    return selectionWindow.SelectedItem;
                }
            }
            return null;
        }
    }

    public class SelectionWindow : Form
    {
        private ListBox _listBox;
        private Button _okButton;

        public SelectionWindow(IEnumerable<string> items)
        {
            Text = "Please select a unit testing framework...";
            Width = 500;
            Height = 100 + items.Count() * 50;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            // Set up the ListBox to display the items
            _listBox = new ListBox
            {
                Dock = DockStyle.Fill,
                SelectionMode = SelectionMode.One
            };
            foreach (var item in items)
            {
                _listBox.Items.Add(item);
            }

            // Set up the OK button
            _okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,

            };
            _okButton.Click += (sender, e) => Close();

            Controls.Add(_listBox);
            Controls.Add(_okButton);

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        
            _okButton.Size = new System.Drawing.Size(_okButton.Width + 20, _okButton.Height);
        }

        public string SelectedItem => _listBox.SelectedItem?.ToString();

    }

    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        private readonly IntPtr _hwnd;
        public WindowWrapper(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }
    }
}
