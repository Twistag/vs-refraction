using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.CreateDocumentation)]
    internal sealed class CreateDocumentationCommand : BaseCommand<CreateDocumentationCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await RefractionBackendService.GenerateAsync("documentation");
        }
    }
}
