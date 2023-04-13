using System;
using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.StyleCheck)]
    internal sealed class StyleCheckCommand : BaseCommand<StyleCheckCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await RefractionBackendService.GenerateAsync("style");
        }
    }
}
