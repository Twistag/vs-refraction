using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refraction
{
    internal class LanguageProps
    {
        public LanguageProps(string value, string label, List<FrameworkProps> frameworks)
        {
            this.value = value;
            this.label = label;
            this.frameworks = frameworks;
        }

        public string value { get; set; }

        public string label { get; set; }

        public List<FrameworkProps> frameworks { get; set; }
    }
}
