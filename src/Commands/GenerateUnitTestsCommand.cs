using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Refracion;

namespace Refraction
{
    [Command(PackageIds.GenerateUnitTests)]
    internal sealed class GenerateUnitTestsCommand : BaseCommand<GenerateUnitTestsCommand>
    {
        protected override async System.Threading.Tasks.Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            List<LanguageProps> languages = await RefractionBackendService.getLanguages();
        }
    }
}
