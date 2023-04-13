using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace Refraction
{
    [Command(PackageIds.RefractionSettings)]
    internal sealed class RefractionSettingsCommand : BaseCommand<RefractionSettingsCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            ShowRefractionSettingsWindow();
        }


        private void ShowRefractionSettingsWindow()
        {
            RefractionSettingsWindow inputWindow = new RefractionSettingsWindow();
     

            // Create an instance of ElementHost to host the WPF window
            ElementHost elementHost = new ElementHost();
            elementHost.Dock = DockStyle.Fill;
            elementHost.Child = inputWindow;

            // Create an instance of Form to be the parent form
            Form form = new Form();
            form.Controls.Add(elementHost);
            form.Text = "Refraction";

            // Show the parent form as a modal dialog
            form.ShowDialog();

          

            // Close the parent form
            //form.Close();
        }
    }
}
