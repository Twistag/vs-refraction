using System.Diagnostics;
using System.Windows;
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
            Window popupWindow = new Window()
            {
                Title = "Refraction Settings", 
                Content = inputWindow,
                Width = 500,
                Height = 140,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen

            };
            popupWindow.ShowDialog();
        }
    }
}
