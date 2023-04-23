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
            string language = GetLanguage();

            return new CodeAndLanguage(code, language);
        }
        
        public static TextSelection GetTextSelection()
        {
            Document activeDoc = getActiveDocument();
            TextDocument textDoc = activeDoc.Object("TextDocument") as TextDocument;     
            TextSelection selection = textDoc.Selection as TextSelection;
            return selection;
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


        public static string GetLanguage()
        {
            Document activeDoc = getActiveDocument();
            string fileExtension = System.IO.Path.GetExtension(activeDoc.FullName);

            

            switch(fileExtension)
            {
                case ".java":
                    {
                        return "java";
                    }
                case ".py": {
                        return "python";
                    }
                case ".cs":
                    {
                        return "csharp";
                    }
                default:
                    {
                        return fileExtension;
                    }
            }
        }

        private static Document getActiveDocument()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            DTE dte = Package.GetGlobalService(typeof(DTE)) as DTE;
            return dte.ActiveDocument;
        }

        private static string GetActiveDocumentLanguage()
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

