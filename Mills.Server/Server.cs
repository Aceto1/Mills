using Mills.Database;
using Mills.Server.Handler;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Mills.Server
{
    internal class Server
    {
        private const string REGISTER = "[Register]\n";
        private const string LOGIN = "[Login]\n";

        private readonly Socket listener;

        private static DatabaseContext databaseContext;

        private static UserHandler userHandler;

        public Server()
        {
            databaseContext = new DatabaseContext();
            userHandler = new UserHandler(databaseContext);

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a Socket that will use Tcp protocol
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // A Socket must be associated with an endpoint using the Bind method
            listener.Bind(localEndPoint);
            // Specify how many requests a Socket can listen before it gives Server busy response.
            // We will listen 10 requests at a time
            listener.Listen(100);

            Console.WriteLine("Server started!");
        }

        public async void StartListening()
        {
            while (true)
            {
                Socket connection = await listener.AcceptAsync();

                HandleConnection(connection);
            }
        }

        private static async void HandleConnection(Socket socket)
        {
            var bytes = new byte[1024];
            var byteCount = await socket.ReceiveAsync(bytes, SocketFlags.None);

            var stringValue = Encoding.UTF8.GetString(bytes, 0, byteCount);

            string result = "";

            if (stringValue.StartsWith(REGISTER))
            {
                var stringValues = stringValue[REGISTER.Length..].Split("\n");
                result = userHandler.Register(stringValues[0], stringValues[1]) ? "OK" : "ERROR";
            }
            else if (stringValue.StartsWith(LOGIN))
            {
                var stringValues = stringValue[LOGIN.Length..].Split("\n");
                result = userHandler.Login(stringValues[0], stringValues[1]) ? "OK" : "ERROR";
            }

            socket.Send(Encoding.UTF8.GetBytes(result));
        }
    }
}
