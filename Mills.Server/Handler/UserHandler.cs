using Mills.Common.Enum;
using Mills.Common.Model;
using Mills.Common.Model.Dto;
using Mills.Database;
using Mills.Database.Entities.User;
using Mills.Server.Global;
using Mills.Server.Model;
using System;
using System.Collections.Generic;
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

            return new RegisteredRequest();
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

            var client = Clients.Instance.GetClient(user.Id);

            if (client != null)
                return new ErrorRequest()
                {
                    Message = "Sie sind bereits in einer anderen Sitzung angemeldet.",
                    Severity = Severity.Error
                };

            var sessionId = Guid.NewGuid().ToString();

            client = Clients.Instance.GetClient(socket);

            if(client == null)
            {
                socket.SendTimeout = 5000;

                var cts = new CancellationTokenSource();

                client = new Client
                {
                    Socket = socket,
                    Cts = cts
                };

                Clients.Instance.AddClient(client);

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (cts.IsCancellationRequested)
                            break;

                        if (!socket.Connected)
                            break;

                        if (socket.GetStream().DataAvailable)
                            Server.HandleConnection(socket);

                        Thread.Sleep(50);
                    }
                });
            }

            client.SessionToken = sessionId;
            client.User = new UserDto
            {
                UserId = user.Id,
                Username = request.Username,
                Status = UserStatus.Online,
            };

            return new LoggedInRequest()
            {
                SessionId = sessionId,
                User = client.User
            };
        }

        public void Logout(LogoutRequest request)
        {
            var client = Clients.Instance.GetClient(request.SessionId);

            if (client != null)
            {
                client.Cts.Cancel();
                Clients.Instance.RemoveClient(request.SessionId);
            }
        }

        public SendActiveUsersRequest GetActiveUsers()
        {
            var clients = Clients.Instance.GetAllClients();

            var users = new List<UserDto>();

            foreach (var client in clients)
            {
                var user = databaseContext.Users.FirstOrDefault(m => m.Id == client.User.UserId);

                users.Add(new UserDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Status = Games.Instance.IsUserIngame(user.Id) ? UserStatus.Ingame : UserStatus.Online,
                });
            }

            return new SendActiveUsersRequest()
            {
                Users = users.ToArray(),
            };
        }
    }
}
