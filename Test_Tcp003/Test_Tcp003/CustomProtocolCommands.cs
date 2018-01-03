using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Tcp003
{
    public class CustomProtocolCommands:IEnumerable<CustomProtocolCommand>
    {
        private readonly List<CustomProtocolCommand> _commands = new List<CustomProtocolCommand>();
        public CustomProtocolCommands()
        {
            string[] commands = { "HELO", "BYE", "SET", "GET", "ECO", "REV" };
            foreach (var command in commands)
            {
                _commands.Add(new CustomProtocolCommand(command));
            }
            _commands.Single(c => c.Name == "HELO").Action = "v1.0";
        }
        public IEnumerator<CustomProtocolCommand> GetEnumerator() => _commands.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _commands.GetEnumerator();

    }
}
