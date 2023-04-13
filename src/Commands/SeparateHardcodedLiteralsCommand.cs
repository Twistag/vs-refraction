using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.SeparateHardcodedLiterals)]
    internal sealed class SeparateHardcodedLiteralsCommand : BaseCommand<SeparateHardcodedLiteralsCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await RefractionBackendService.GenerateAsync("literals");
        }
    }
}
