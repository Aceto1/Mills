using System.Net.Sockets;

namespace Mills.Server.Model
{
    public class Client
    {
        public int UserId { get; set; }

        public string SessionToken { get; set; }

        public Socket Socket { get; set; }
    }
}
