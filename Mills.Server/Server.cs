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
using System.Threading;
using System.Threading.Tasks;

namespace Mills.Server
{
    internal class Server
    {
        private readonly TcpListener listener;

        private static DatabaseContext databaseContext;

        private static UserHandler userHandler;

        public Server()
        {
            databaseContext = new DatabaseContext();
            userHandler = new UserHandler(databaseContext);

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

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
            catch (Exception)
            {
                var client = Clients.Instance.GetClient(socket);
                Clients.Instance.RemoveClient(client.SessionToken);
            }

            var request = bytes.Deserialize(byteCount);

            List<Tuple<TcpClient, Request>> responses = new List<Tuple<TcpClient, Request>>();

            switch (request?.Method)
            {
                case RequestMethod.Login:
                    responses.Add(new Tuple<TcpClient, Request>(socket, userHandler.Login(request as LoginRequest, socket)));

                    if (responses[0].Item2 is LoggedInRequest loggedInRequest)
                    {
                        responses.Add(new Tuple<TcpClient, Request>(socket, ChallengeHandler.GetChallengesForUser(loggedInRequest.User.UserId)));
                        var clients = Clients.Instance.GetAllClients().Where(m => m.User != null);

                        foreach (var client in clients)
                        {
                            responses.Add(new Tuple<TcpClient, Request>(client.Socket, userHandler.GetActiveUsers()));
                        }
                    }
                    break;
                case RequestMethod.Logout:
                    userHandler.Logout(request as LogoutRequest);
                    break;
                case RequestMethod.Register:
                    responses.Add(new Tuple<TcpClient, Request>(socket, userHandler.Register(request as RegisterRequest)));
                    break;
                case RequestMethod.Challenge:
                    if (ChallengeHandler.AddChallenge(request as ChallengeRequest))
                    {
                        responses.Add(new Tuple<TcpClient, Request>(socket, ChallengeHandler.GetChallengesForUser((request as ChallengeRequest).FromUserId)));

                        var client = Clients.Instance.GetClient((request as ChallengeRequest).ToUserId);

                        responses.Add(new Tuple<TcpClient, Request>(client.Socket, ChallengeHandler.GetChallengesForUser((request as ChallengeRequest).ToUserId)));
                    }
                    else
                        responses.Add(new Tuple<TcpClient, Request>(socket, new ErrorRequest { Message = "Es gibt bereits eine offene Herausforderung zwischen Ihnen.", Severity = Severity.Information }));
                    break;
                case RequestMethod.ChallengeAccepted:
                    {
                        var client1 = Clients.Instance.GetClient((request as ChallengeAcceptedRequest).FromUserId);
                        var client2 = Clients.Instance.GetClient((request as ChallengeAcceptedRequest).ToUserId);

                        if(client1.User.Status == UserStatus.Ingame || client2.User.Status == UserStatus.Ingame)
                        {
                            responses.Add(new Tuple<TcpClient, Request>(socket, new ErrorRequest
                            {
                                Message = "Der Nutzer befindet sich bereits in einem Spiel.",
                                Severity = Severity.Information
                            }));
                            break;
                        }

                        if (ChallengeHandler.AcceptChallenge(request as ChallengeAcceptedRequest))
                        {
                            responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new GameStartedRequest()
                            {
                                AgainstUser = client2.User,
                                Starting = false
                            }));
                            responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new GameStartedRequest()
                            {
                                AgainstUser = client1.User,
                                Starting = true
                            }));

                            client1.User.Status = UserStatus.Ingame;
                            client2.User.Status = UserStatus.Ingame;

                            GameHandler.StartGame(request as ChallengeAcceptedRequest);
                        }
                        else
                        {
                            responses.Add(new Tuple<TcpClient, Request>(socket, new ErrorRequest
                            {
                                Message = "Die Herausforderung wurde zurückgezogen.",
                                Severity = Severity.Information
                            }));
                        }

                        responses.Add(new Tuple<TcpClient, Request>(client1.Socket, ChallengeHandler.GetChallengesForUser((request as ChallengeAcceptedRequest).FromUserId)));
                        responses.Add(new Tuple<TcpClient, Request>(client2.Socket, ChallengeHandler.GetChallengesForUser((request as ChallengeAcceptedRequest).ToUserId)));
                    }
                    break;
                case RequestMethod.ChallengeCancelled:
                    {
                        ChallengeHandler.CancelChallenge(request as ChallengeCancelledRequest);

                        var client1 = Clients.Instance.GetClient((request as ChallengeCancelledRequest).FromUserId);
                        var client2 = Clients.Instance.GetClient((request as ChallengeCancelledRequest).ToUserId);

                        responses.Add(new Tuple<TcpClient, Request>(client1.Socket, ChallengeHandler.GetChallengesForUser((request as ChallengeCancelledRequest).FromUserId)));
                        responses.Add(new Tuple<TcpClient, Request>(client2.Socket, ChallengeHandler.GetChallengesForUser((request as ChallengeCancelledRequest).ToUserId)));
                    }
                    break;
                case RequestMethod.Place:
                    if (GameHandler.Place(request as PlaceRequest, out var remove, out var phaseChange))
                    {
                        var game = Games.Instance.GetGameBySessionId((request as PlaceRequest).SessionId);
                        var client1 = Clients.Instance.GetClient(game.UserId1);
                        var client2 = Clients.Instance.GetClient(game.UserId2);

                        responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new PlacedRequest
                        {
                            Position = (request as PlaceRequest).Position,
                            Remove = remove,
                            PhaseChange = phaseChange
                        }));

                        responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new PlacedRequest
                        {
                            Position = (request as PlaceRequest).Position,
                            Remove = remove,
                            PhaseChange = phaseChange
                        }));

                        if (phaseChange && !remove)
                        {
                            if (!MillsHelper.HasAvailableMoves(game.BoardState, game.User1TokenCount, PositionState.Player1))
                            {
                                responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new LoseRequest()));
                                responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new WinRequest()));
                                Games.Instance.RemoveGame(game);
                                client1.User.Status = UserStatus.Online;
                                client2.User.Status = UserStatus.Online;
                            }
                            else if (!MillsHelper.HasAvailableMoves(game.BoardState, game.User2TokenCount, PositionState.Player2))
                            {
                                responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new WinRequest()));
                                responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new LoseRequest()));
                                Games.Instance.RemoveGame(game);
                                client1.User.Status = UserStatus.Online;
                                client2.User.Status = UserStatus.Online;
                            }
                        }
                    }
                    break;
                case RequestMethod.Remove:
                    if (GameHandler.Remove(request as RemoveRequest))
                    {
                        var game = Games.Instance.GetGameBySessionId((request as RemoveRequest).SessionId);
                        var client1 = Clients.Instance.GetClient(game.UserId1);
                        var client2 = Clients.Instance.GetClient(game.UserId2);

                        responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new RemovedRequest
                        {
                            Position = (request as RemoveRequest).Position
                        }));

                        responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new RemovedRequest
                        {
                            Position = (request as RemoveRequest).Position
                        }));

                        game.SwitchActivePlayer();

                        if (game.Phase > 1 && game.User1TokenCount < 3)
                        {
                            responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new LoseRequest()));
                            responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new WinRequest()));
                            Games.Instance.RemoveGame(game);
                            client1.User.Status = UserStatus.Online;
                            client2.User.Status = UserStatus.Online;
                        }
                        else if (game.Phase > 1 && game.User2TokenCount < 3)
                        {
                            responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new WinRequest()));
                            responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new LoseRequest()));
                            Games.Instance.RemoveGame(game);
                            client1.User.Status = UserStatus.Online;
                            client2.User.Status = UserStatus.Online;
                        }
                    }
                    break;
                case RequestMethod.Move:
                    if (GameHandler.Move(request as MoveRequest, out var withRemove))
                    {
                        var game = Games.Instance.GetGameBySessionId((request as MoveRequest).SessionId);
                        var client1 = Clients.Instance.GetClient(game.UserId1);
                        var client2 = Clients.Instance.GetClient(game.UserId2);

                        responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new MovedRequest
                        {
                            From = (request as MoveRequest).From,
                            To = (request as MoveRequest).To,
                            Remove = withRemove
                        }));

                        responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new MovedRequest
                        {
                            From = (request as MoveRequest).From,
                            To = (request as MoveRequest).To,
                            Remove = withRemove
                        }));

                        if (!withRemove)
                        {
                            if (!MillsHelper.HasAvailableMoves(game.BoardState, game.User1TokenCount, PositionState.Player1))
                            {
                                responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new LoseRequest()));
                                responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new WinRequest()));
                                Games.Instance.RemoveGame(game);
                                client1.User.Status = UserStatus.Online;
                                client2.User.Status = UserStatus.Online;
                            }
                            else if (!MillsHelper.HasAvailableMoves(game.BoardState, game.User2TokenCount, PositionState.Player2))
                            {
                                responses.Add(new Tuple<TcpClient, Request>(client1.Socket, new WinRequest()));
                                responses.Add(new Tuple<TcpClient, Request>(client2.Socket, new LoseRequest()));
                                Games.Instance.RemoveGame(game);
                                client1.User.Status = UserStatus.Online;
                                client2.User.Status = UserStatus.Online;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            for (int i = 0; i < responses.Count; i++)
            {
                responses[i].Item1.GetStream().Write(responses[i].Item2.SerializeToBytes());

                if (i != responses.Count - 1)
                    Thread.Sleep(50);
            }
        }
    }
}