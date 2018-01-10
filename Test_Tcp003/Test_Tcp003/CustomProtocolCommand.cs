using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Tcp003
{
    /// <summary>
    /// 用户界面的数据绑定
    /// </summary>
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
        /// <summary>
        /// 显示命令的名称
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 用户输入的、与命令一起发送的数据
        /// </summary>
        public string Action { get; set; }
        //public override string ToString() => Name;
        public override string ToString()
        {
            return Name;
        }
    }
}
