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
    {
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

        private async void OnConnect(object sender, EventArgs e)//RoutedEventArgs e)
        {
            try
            {
                await _client.ConnectAsync(RemoteHost, ServerPort);
            }
            catch (SocketException ex) when (ex.ErrorCode==0x2748)
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

        private async void OnSendCommand(object sender,EventArgs e)
        {
            try
            {
                if (!VerifyIsConnected()) return;
                NetworkStream stream = _client.GetStream();
                byte[] writeBuffer = Encoding.ASCII.GetBytes(GetCommand());
                await stream.WriteAsync(writeBuffer, 0, writeBuffer.Length);
                await stream.FlushAsync();
                byte[] readBuffer = new byte[1024];
                int read = await stream.ReadAsync(readBuffer, 0, readBuffer.Length);
                string messageRead = Encoding.ASCII.GetString(readBuffer, 0, read);
                Log += messageRead + Environment.NewLine;
                ParseMessage(messageRead);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetCommand() => $"{GetSessionHeader()}{ActiveCommand?.Name}::{ActiveCommand?.Action}";
        private string GetSessionHeader()
        {
            if (string.IsNullOrEmpty(SessionId)) return string.Empty;
            return $"ID::{SessionId}::";
        }

        private void ParseMessage(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            string[] messageColl = message.Split(
                new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            Status = messageColl[0];
            SessionId = GetSessionId(messageColl);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
