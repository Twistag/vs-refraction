using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refraction
{
    internal class CodeAndLanguage
    {
        public CodeAndLanguage(string code, string language)
        {
            this.code = code;
            this.language = language;
        }

        public string code { get; }

        public string language { get; }
    }
}
