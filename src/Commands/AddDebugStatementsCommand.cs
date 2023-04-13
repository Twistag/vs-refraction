using System;
using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.AddDebugStatements)]
    internal sealed class AddDebugStatementsCommand : BaseCommand<AddDebugStatementsCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await RefractionBackendService.GenerateAsync("debug");
        }
    }
}
