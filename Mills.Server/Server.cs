using Mills.Database;
using Mills.Server.Handler;
using Mills.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
        private readonly Socket listener;

        private static DatabaseContext databaseContext;

        private static UserHandler userHandler;

        private static List<Client> clients = new List<Client>();

        private static List<Game> games = new List<Game>();

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

            var request = new Request(stringValue);
        }
    }
}