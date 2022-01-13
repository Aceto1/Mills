using Mills.Common.Model.Dto;
using System.Net.Sockets;
using System.Threading;

namespace Mills.Server.Model
{
    public class Client
    {
        public UserDto User { get; set; }

        public string SessionToken { get; set; }

        public TcpClient Socket { get; set; }

        public CancellationTokenSource Cts { get; set; }
    }
}
