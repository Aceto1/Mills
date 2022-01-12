using Mills.Common.Enum;
using Mills.Common.Model;
using Mills.Database;
using Mills.Database.Entities.User;
using Mills.Server.Global;
using Mills.Server.Model;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mills.Server.Handler
{
    internal class UserHandler
    {
        private readonly DatabaseContext databaseContext;

        public UserHandler(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public Request Register(RegisterRequest request)
        {
            var user = databaseContext.Users.FirstOrDefault(m => m.Username == request.Username);

            if (user != null)
                return new ErrorRequest()
                {
                    Message = "Benutzer mit diesem Namen existiert bereits.",
                    Severity = Severity.Error
                };

            user = new User()
            {
                Username = request.Username,
                Password = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(request.Password)))
            };

            databaseContext.Users.Add(user);
            databaseContext.SaveChanges();

            return new OkRequest();
        }

        public Request Login(LoginRequest request, TcpClient socket)
        {
            var user = databaseContext.Users.FirstOrDefault(m => m.Username == request.Username);

            if (user == null)
                return new ErrorRequest()
                {
                    Message = "Benutzer mit diesem Namen existiert nicht.",
                    Severity = Severity.Error
                };

            var hashedPw = Encoding.UTF8.GetString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(request.Password)));

            if (user.Password != hashedPw)
                return new ErrorRequest()
                {
                    Message = "Das Passwort ist nicht korrekt.",
                    Severity = Severity.Error
                };

            var sessionId = Guid.NewGuid().ToString();

            return new OkRequest()
            {
                SessionId = sessionId
            };
        }

        public void Logout(LogoutRequest request)
        {
            var client = Clients.Instance.GetClient(request.SessionId);

            if(client != null)
            {
                client.Cts.Cancel();
                Clients.Instance.RemoveClient(request.SessionId);
            }
        }
    }
}
