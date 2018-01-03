using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Tcp003
{
    public class CustomProtocolCommand
    {
        public CustomProtocolCommand(string name)
            : this(name, null)
        {
        }
        public CustomProtocolCommand(string name, string action)
        {
            Name = name;
            Action = action;
        }
        public string Name { get; }
        public string Action { get; set; }
        public override string ToString() => Name;
    }
}
