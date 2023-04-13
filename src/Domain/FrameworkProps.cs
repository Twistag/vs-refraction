using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refraction
{
    internal class FrameworkProps
    {
        public FrameworkProps(string value, string label)
        {
            this.value = value;
            this.label = label;
        }

        public string value { get; set; }

        public string label { get; set; }
    }
}
