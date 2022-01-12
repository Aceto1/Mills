using Mills.Common.Enum;
using Mills.Common.Helper;
using Mills.Common.Model;
using Mills.Database;
using Mills.Server.Global;
using Mills.Server.Handler;
using Mills.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mills.Server
{
    // Neue Struktur Anfagen:
    /* Beispiel:
     * [LOGIN] -> Methode
     * username=Atze -> Feld
     * password=12345 -> Feld
     */
    // Antwort:
    /*
     * [OK] -> Status
     * sessiontoken={Guid} -> Feld
     */
    // Oder:
    /*
     * [ERROR]
     * message="Incorrent username or password."
     */

    //TODO: Parser und Models/Enums für Anfragen bauen
    internal class Server
    {
        private readonly TcpListener listener;

        private static DatabaseContext databaseContext;

        private static UserHandler userHandler;

        private static List<Game> games = new List<Game>();

        public Server()
        {
            databaseContext = new DatabaseContext();
            userHandler = new UserHandler(databaseContext);

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a Socket that will use Tcp protocol
            listener = new TcpListener(localEndPoint);
            listener.Start();

            Console.WriteLine("Server started!");
        }

        public async void StartListening()
        {
            while (true)
            {
                TcpClient connection = await listener.AcceptTcpClientAsync();

                HandleConnection(connection);
            }
        }

        public static async void HandleConnection(TcpClient socket)
        {
            if (Clients.Instance.GetClient(socket) == null)
            {
                socket.SendTimeout = 5000;

                var cts = new CancellationTokenSource();

                Clients.Instance.AddClient(new Client
                {
                    Socket = socket,
                    Cts = cts
                });

                await Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (cts.IsCancellationRequested)
                            break;

                        if (socket.GetStream().DataAvailable)
                            HandleConnection(socket);

                        Thread.Sleep(50);
                    }
                });
            }

            var bytes = new byte[1024];
            int byteCount = 0;

            try
            {
                byteCount = await socket.GetStream().ReadAsync(bytes);
            }
            catch (InvalidOperationException)
            {
                var client = Clients.Instance.GetClient(socket);
                Clients.Instance.RemoveClient(client.SessionToken);
            }

            var stringValue = Encoding.UTF8.GetString(bytes, 0, byteCount).Trim('\0');

            var request = RequestHelper.ParseRequest(stringValue);

            Request response = null;

            switch (request?.Method)
            {
                case RequestMethod.Login:
                    response = userHandler.Login(request as LoginRequest, socket);
                    break;
                case RequestMethod.Logout:
                    userHandler.Logout(request as LogoutRequest);
                    break;
                case RequestMethod.Register:
                    response = userHandler.Register(request as RegisterRequest);
                    break;
                default:
                    break;
            }

            if (response != null)
                socket.GetStream().Write(Encoding.UTF8.GetBytes(response.ToString()));
        }
    }
}