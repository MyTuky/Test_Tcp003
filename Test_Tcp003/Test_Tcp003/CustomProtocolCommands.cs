using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Tcp003
{
    /// <summary>
    /// 用户界面的数据绑定
    /// </summary>
    public class CustomProtocolCommands:IEnumerable<CustomProtocolCommand>
    {
        /// <summary>
        /// 绑定到组合框的命令列表
        /// </summary>
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
        //public IEnumerator<CustomProtocolCommand> GetEnumerator() => _commands.GetEnumerator();
        public IEnumerator<CustomProtocolCommand> GetEnumerator()
        {
            return _commands.GetEnumerator();
        }

        //IEnumerator IEnumerable.GetEnumerator() => _commands.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _commands.GetEnumerator();
        }
    }
}
