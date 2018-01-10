using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Tcp003
{
    public partial class MainWindow : Form, INotifyPropertyChanged, IDisposable
    {//包含绑定到XAML代码的属性和基于用户交互调用的方法，一些绑定到用户界面的属性
        private TcpClient _client = new TcpClient();
        private readonly CustomProtocolCommands _commands = new CustomProtocolCommands();

        public MainWindow()
        {
            InitializeComponent();
        }

        private string _remoteHost = "localhost";
        public string RemoteHost
        {
            get { return _remoteHost; }
            set { _remoteHost = value; }// SetProperty(ref _remoteHost, value); }
        }

        private int _serverport = 8800;
        public int ServerPort
        {
            get { return _serverport; }
            set { _serverport = value; }//SetProperty(ref _serverport, value); }
        }

        private string _sessionId;
        public string SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }//SetProperty(ref _sessionId, value); }
        }

        private CustomProtocolCommand _activeCommand;
        public CustomProtocolCommand ActiveCommand
        {
            get { return _activeCommand; }
            set { _activeCommand = value; }//SetProperty(ref _activeCommand, value); }
        }

        private string _log;
        public string Log
        {
            get { return _log; }
            set { _log = value; }//SetProperty(ref _log, value); }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set { _status = value; }//SetProperty(ref _status, value); }
        }
        /// <summary>
        /// 建立到TCP服务器的连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnConnect(object sender, EventArgs e)//RoutedEventArgs e)
        {
            try
            {
                await _client.ConnectAsync(RemoteHost, ServerPort);
            }
            catch (SocketException ex) when (ex.ErrorCode==0x2748)//ErrorCode设置为0x2748
            {
                _client.Close();
                _client = new TcpClient();
                MessageBox.Show("please retry connect");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 请求发送到TCP服务器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSendCommand(object sender,EventArgs e)
        {
            try
            {
                if (!VerifyIsConnected()) return;
                NetworkStream stream = _client.GetStream();//获取连接服务器的流通道
                byte[] writeBuffer = Encoding.ASCII.GetBytes(GetCommand());
                await stream.WriteAsync(writeBuffer, 0, writeBuffer.Length);//将数据写入流，由流写入服务器
                await stream.FlushAsync();
                byte[] readBuffer = new byte[1024];
                int read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);//从服务器读取数据
                string messageRead = Encoding.ASCII.GetString(readBuffer, 0, read);
                Log += messageRead + Environment.NewLine;
                ParseMessage(messageRead);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 建立可以发送到服务器的数据
        /// </summary>
        /// <returns></returns>
        private string GetCommand()
        {
            return $"{GetSessionHeader()}{ActiveCommand?.Name}::{ActiveCommand?.Action}";
        }

        //private string GetCommand() => $"{GetSessionHeader()}{ActiveCommand?.Name}::{ActiveCommand?.Action}";
        /// <summary>
        /// 建立会话标识符
        /// </summary>
        /// <returns></returns>
        private string GetSessionHeader()
        {
            if (string.IsNullOrEmpty(SessionId)) return string.Empty;
            return $"ID::{SessionId}::";
        }
        /// <summary>
        /// 拆分消息，设置Status和SessionId
        /// </summary>
        /// <param name="message"></param>
        private void ParseMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            string[] messageColl = message.Split(
                new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            Status = messageColl[0];
            SessionId = GetSessionId(messageColl);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #region 猜的
        bool VerifyIsConnected()
        {
            return _client.Connected;
        }
        string GetSessionId(string[] messageColl)
        {
            if (messageColl[1]=="ID")
            {
                return messageColl[2];
            }
            return string.Empty;
        }
        #endregion
    }
}
