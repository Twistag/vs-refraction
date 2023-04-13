using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Windows.Controls;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.LanguageServices;

namespace Refraction
{
    internal class VSServices
    {
        public static CodeAndLanguage getCodeAndLanguage()
        {
            string code = getSelectedText();
            string language = getLanguage();

            return new CodeAndLanguage(code, language);
        }

        public static void InsertText(string text)
        {
            Document activeDoc = getActiveDocument();
            TextDocument textDoc = activeDoc.Object("TextDocument") as TextDocument;     
            TextSelection selection = textDoc.Selection as TextSelection;
            selection.MoveToAbsoluteOffset(selection.ActivePoint.CreateEditPoint().AbsoluteCharOffset + selection.Text.Length, false);
            selection.Insert(text);
        }

        private static string getSelectedText()
        {
            Document activeDoc = getActiveDocument();
            if (activeDoc != null)
            {
                TextSelection textSelection = activeDoc.Selection as TextSelection;
                if (textSelection != null)
                {
                    return textSelection.Text;
                }
            }

            
            return null;
        }


        private static string getLanguage()
        {
            Document activeDoc = getActiveDocument();
            TextDocument textDoc = activeDoc.Object("TextDocument") as TextDocument;

            if (activeDoc == null)
            {
                return null;
            }
            
            return textDoc.Language;
        }

        private static Document getActiveDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            return dte.ActiveDocument;
        }

        public static string GetActiveDocumentLanguage()
        {
  

            IVsTextManager textManager = (IVsTextManager)Package.GetGlobalService(typeof(SVsTextManager));
            Document activeDoc = getActiveDocument();
            string fileExtension = System.IO.Path.GetExtension(activeDoc.FullName);
            Guid languageGuid;
            textManager.MapFilenameToLanguageSID(fileExtension, out languageGuid);
            IVsLanguageInfo languageInfo = (IVsLanguageInfo)Package.GetGlobalService(Type.GetTypeFromCLSID(languageGuid));
            string languageName;
            languageInfo.GetLanguageName(out languageName);
            return languageName;

        }
    }
}

