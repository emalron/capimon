using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capimon
{
    public class Element
    {
        public Element(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
        public string name;
        public string value;
    }
}
