using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.DetectBugs)]
    internal sealed class DetectBugsCommand : BaseCommand<DetectBugsCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await RefractionBackendService.GenerateAsync("bugs");
        }
    }
}
