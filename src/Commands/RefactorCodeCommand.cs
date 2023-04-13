using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace Refraction
{
    [Command(PackageIds.RefactorCode)]
    internal sealed class RefactorCodeCommand : BaseCommand<RefactorCodeCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await RefractionBackendService.GenerateAsync("refactor");
        }
    }
}
