using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;

namespace Refraction
{
    [Command(PackageIds.AddTypes)]
    internal sealed class AddTypesCommand : BaseCommand<AddTypesCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
             await RefractionBackendService.GenerateAsync("types");
        }

        
    }
}
