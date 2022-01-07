using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Mills.Model
{
    public static class ServerConnection
    {
        private static readonly Socket socket;

        static ServerConnection()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a Socket that will use Tcp protocol
            socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(localEndPoint);
        }

        public static bool Register(string username, string password)
        {
            var toSend = $"[REGISTER]\n{username}\n{password}";
            socket.Send(Encoding.UTF8.GetBytes(toSend));

            var result = new byte[1024];
            var byteCount = socket.Receive(result);

            return Encoding.UTF8.GetString(result, 0, byteCount) == "OK";
        }

        public static bool Login(string username, string password)
        {
            var toSend = $"[LOGIN]\n{username}\n{password}";
            socket.Send(Encoding.UTF8.GetBytes(toSend));

            var result = new byte[1024];
            var byteCount = socket.Receive(result);

            return Encoding.UTF8.GetString(result, 0, byteCount) == "OK";
        }
    }
}
