using Mills.Common.Enum;
using Mills.Common.Helper;
using Mills.Common.Model;
using Mills.Common.Model.Dto;
using Mills.View;
using Mills.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Mills.Model
{
    public class ServerConnection
    {
        private readonly TcpClient socket;

        private string sessionId;

        private static ServerConnection instance;

        public static ServerConnection Instance => instance ?? (instance = new ServerConnection());

        private ServerConnection()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            socket = new TcpClient();
            socket.Connect(localEndPoint);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (socket.GetStream().DataAvailable)
                    {
                        HandleConnection();
                    }

                    Thread.Sleep(50);
                }
            });
        }

        private async void HandleConnection()
        {
            var bytes = new byte[1024];
            var byteCount = await socket.GetStream().ReadAsync(bytes);

            var request = bytes.Deserialize(byteCount);

            switch (request.Method)
            {
                case RequestMethod.Error:
                    MainViewModel.Instance.ShowMessage((request as ErrorRequest)?.Message, (request as ErrorRequest)?.Severity ?? Severity.Error);
                    break;
                case RequestMethod.LoggedIn:
                    MainViewModel.Instance.CurrentUser = (request as LoggedInRequest).User;
                    sessionId = (request as LoggedInRequest).SessionId;
                    MainViewModel.Instance.SwitchPage(this, nameof(View.Lobby));
                    break;
                case RequestMethod.Registered:
                    MainViewModel.Instance.SwitchPage(this, nameof(View.Login));
                    break;
                case RequestMethod.SendActiveUsers:
                    LobbyViewModel.Instance.Users = new ObservableCollection<UserDto>((request as SendActiveUsersRequest).Users.Where(m => m.UserId != MainViewModel.Instance.CurrentUser.UserId));
                    break;
                case RequestMethod.SendChallenges:
                    LobbyViewModel.Instance.MyChallenges = new ObservableCollection<ChallengeDto>((request as SendChallengesRequest).MyChallenges);
                    LobbyViewModel.Instance.ChallengesAgainstMe = new ObservableCollection<ChallengeDto>((request as SendChallengesRequest).ChallengesAgainstMe);
                    break;
                case RequestMethod.GameStarted:
                    MainViewModel.Instance.SwitchPage(this, nameof(Game));
                    // Spielfeld zurücksetzen
                    GameViewModel.Instance.Reset();

                    // Wenn man startet ist man Spieler 1
                    if ((request as GameStartedRequest).Starting)
                        GameViewModel.Instance.OwnColor = PositionState.Player1;
                    else
                        GameViewModel.Instance.OwnColor = PositionState.Player2;
                    break;
                case RequestMethod.SendMessage:
                    break;
                case RequestMethod.Placed:
                    GameViewModel.Instance.BoardState.Add((request as PlacedRequest).Position, GameViewModel.Instance.ActivePlayer);
                    GameViewModel.Instance.OnPropertyChanged(nameof(GameViewModel.Instance.BoardState));
                    if ((request as PlacedRequest).Remove)
                        GameViewModel.Instance.Remove = true;
                    else
                        GameViewModel.Instance.SwitchPlayers();

                    if ((request as PlacedRequest).PhaseChange)
                        GameViewModel.Instance.Phase++;
                    break;
                case RequestMethod.Moved:
                    GameViewModel.Instance.BoardState.Remove((request as MovedRequest).From);
                    GameViewModel.Instance.BoardState.Add((request as MovedRequest).To, GameViewModel.Instance.ActivePlayer);
                    GameViewModel.Instance.OnPropertyChanged(nameof(GameViewModel.Instance.BoardState));
                    if ((request as MovedRequest).Remove)
                        GameViewModel.Instance.Remove = true;
                    else
                        GameViewModel.Instance.SwitchPlayers();
                    break;
                case RequestMethod.Removed:
                    GameViewModel.Instance.BoardState.Remove((request as RemovedRequest).Position);
                    GameViewModel.Instance.OnPropertyChanged(nameof(GameViewModel.Instance.BoardState));
                    GameViewModel.Instance.Remove = false;
                    GameViewModel.Instance.SwitchPlayers();
                    break;
                case RequestMethod.Lose:
                    MainViewModel.Instance.ShowMessage("Sie haben das Spiel verloren!", Severity.Information);
                    MainViewModel.Instance.SwitchPage(this, nameof(Lobby));
                    // Spielfeld zurücksetzen
                    GameViewModel.Instance.Reset();
                    break;
                case RequestMethod.Win:
                    MainViewModel.Instance.ShowMessage("Sie haben das Spiel gewonnen!", Severity.Information);
                    MainViewModel.Instance.SwitchPage(this, nameof(Lobby));
                    // Spielfeld zurücksetzen
                    GameViewModel.Instance.Reset();
                    break;
                default:
                    break;
            }
        }

        public void Register(string username, string password)
        {
            var request = new RegisterRequest();

            request.Username = username;
            request.Password = password;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void Login(string username, string password)
        {
            if (sessionId != null)
            {
                MainViewModel.Instance.ShowMessage("Sie sind bereits eingeloggt.", Severity.Information);
                return;
            }

            var request = new LoginRequest();

            request.Username = username;
            request.Password = password;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void Logout()
        {
            if (sessionId == null)
                return;

            var request = new LogoutRequest();

            request.SessionId = sessionId;

            socket.GetStream().Write(request.SerializeToBytes());

            sessionId = null;
        }

        public void Shutdown()
        {
            Logout();

            socket.Close();
        }

        public void Challenge(UserDto user)
        {
            var request = new ChallengeRequest();

            request.FromUserId = MainViewModel.Instance.CurrentUser.UserId;
            request.ToUserId = user.UserId;
            request.SessionId = sessionId;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void AcceptChallenge(ChallengeDto challenge)
        {
            var request = new ChallengeAcceptedRequest();

            request.FromUserId = challenge.FromUserId;
            request.ToUserId = challenge.ToUserId;
            request.SessionId = sessionId;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void CancelChallenge(ChallengeDto challenge)
        {
            var request = new ChallengeCancelledRequest();

            request.FromUserId = challenge.FromUserId;
            request.ToUserId = challenge.ToUserId;
            request.SessionId = sessionId;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void Place(BoardPosition position)
        {
            var request = new PlaceRequest();

            request.Position = position;
            request.SessionId = sessionId;
            request.Player = MainViewModel.Instance.CurrentUser;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void Move(BoardPosition from, BoardPosition to)
        {
            var request = new MoveRequest();

            request.From = from;
            request.To = to;
            request.SessionId = sessionId;

            socket.GetStream().Write(request.SerializeToBytes());
        }

        public void Remove(BoardPosition position)
        {
            var request = new RemoveRequest();

            request.Position = position;
            request.SessionId = sessionId;

            socket.GetStream().Write(request.SerializeToBytes());
        }
    }
}
