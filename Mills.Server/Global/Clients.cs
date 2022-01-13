using Mills.Server.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Mills.Server.Global
{
    public class Clients
    {
        private Clients()
        {

        }

        private List<Client> clients = new List<Client>();

        private static Clients instance;

        public static Clients Instance => instance ?? (instance = new Clients());

        public void AddClient(Client client)
        {
            clients.Add(client);
        }

        public void RemoveClient(string sessionId)
        {
            var client = clients.FirstOrDefault(m => m.SessionToken == sessionId);

            if(clients != null)
                clients.Remove(client);
        }

        public Client GetClient(string sessionId)
        {
            return clients.FirstOrDefault(m => m.SessionToken == sessionId);
        }

        public Client GetClient(TcpClient client)
        {
            return clients.FirstOrDefault(m => m.Socket == client);
        }

        public Client GetClient(int userId)
        {
            return clients.FirstOrDefault(m => m.User?.UserId == userId);
        }

        public List<Client> GetAllClients()
        {
            return clients;
        }
    }
}
