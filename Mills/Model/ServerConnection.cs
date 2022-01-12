using Mills.Common.Enum;
using Mills.Common.Helper;
using Mills.Common.Model;
using Mills.ViewModel;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mills.Model
{
    public class ServerConnection
    {
        private readonly TcpClient socket;

        private string sessionId;

        private static ServerConnection instance;

        public static ServerConnection Instance => instance ?? (instance = new ServerConnection());

        private ServerConnection()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a Socket that will use Tcp protocol
            socket = new TcpClient();
            socket.Connect(localEndPoint);

            Task.Factory.StartNew(() =>
            {
                if (socket.GetStream().DataAvailable)
                {
                    HandleConnection();
                }

                Thread.Sleep(50);
            });
        }

        private async void HandleConnection()
        {
            var bytes = new byte[1024];
            var byteCount = await socket.GetStream().ReadAsync(bytes);
            var stringValue = Encoding.UTF8.GetString(bytes, 0, byteCount).Trim('\0');

            var request = RequestHelper.ParseRequest(stringValue);
        }

        public bool Register(string username, string password)
        {
            var request = new RegisterRequest();

            request.Username = username;
            request.Password = password;

            socket.GetStream().Write(Encoding.UTF8.GetBytes(request.ToString()));

            var result = new byte[1024];
            socket.GetStream().Read(result);

            var response = RequestHelper.ParseRequest(Encoding.UTF8.GetString(result));

            if (response.Method == RequestMethod.Ok)
                return true;

            MainViewModel.Instance.ShowMessage((response as ErrorRequest)?.Message, (response as ErrorRequest)?.Severity ?? Severity.Error);

            return false;
        }

        public bool Login(string username, string password)
        {
            if (sessionId != null)
                return false;

            var request = new LoginRequest();

            request.Username = username;
            request.Password = password;

            socket.GetStream().Write(Encoding.UTF8.GetBytes(request.ToString()));

            var result = new byte[1024];
            socket.GetStream().Read(result);

            var response = RequestHelper.ParseRequest(Encoding.UTF8.GetString(result));

            if (response.Method == RequestMethod.Ok)
            {
                sessionId = ((OkRequest)response).SessionId;
                return true;
            }

            MainViewModel.Instance.ShowMessage((response as ErrorRequest)?.Message, (response as ErrorRequest)?.Severity ?? Severity.Error);

            return false;
        }

        public void Logout()
        {
            if (sessionId == null)
                return;

            var request = new LogoutRequest();

            request.SessionId = sessionId;

            socket.GetStream().Write(Encoding.UTF8.GetBytes(request.ToString()));

            sessionId = null;
        }

        public void Shutdown()
        {
            Logout();

            socket.Close();
        }
    }
}
