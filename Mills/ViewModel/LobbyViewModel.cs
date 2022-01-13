using Mills.Common.Model.Dto;
using Mills.Model;
using Mills.View;
using Mills.ViewModel.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mills.ViewModel
{
    public class LobbyViewModel : INotifyPropertyChanged
    {
        private LobbyViewModel()
        {
            Users = new ObservableCollection<UserDto>();
            MyChallenges = new ObservableCollection<ChallengeDto>();
            ChallengesAgainstMe = new ObservableCollection<ChallengeDto>();
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Löst das PropertyChanged-Event mit dem angegebenen Propertynamen aus.
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Fields

        private static LobbyViewModel instance;
        private ObservableCollection<UserDto> users;
        private ObservableCollection<ChallengeDto> myChallenges;
        private ObservableCollection<ChallengeDto> challengesAgainstMe;
        private bool isLoading;

        #endregion

        #region Properties

        public ObservableCollection<UserDto> Users
        {
            get => users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public ObservableCollection<ChallengeDto> MyChallenges
        {
            get => myChallenges;
            set
            {
                myChallenges = value;
                OnPropertyChanged(nameof(MyChallenges));
            }
        }

        public ObservableCollection<ChallengeDto> ChallengesAgainstMe
        {
            get => challengesAgainstMe;
            set
            {
                challengesAgainstMe = value;
                OnPropertyChanged(nameof(ChallengesAgainstMe));
            }
        }

        public bool IsLoading 
        { 
            get => isLoading; 
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public static LobbyViewModel Instance => instance ?? (instance = new LobbyViewModel());

        #endregion

        #region Methods

        public void Challenge(UserDto user)
        {
            ServerConnection.Instance.Challenge(user);
        }

        public void AcceptChallenge(ChallengeDto challenge)
        {
            ServerConnection.Instance.AcceptChallenge(challenge);
        }

        public void CancelChallenge(ChallengeDto challenge)
        {
            ServerConnection.Instance.CancelChallenge(challenge);
        }

        public void Logout()
        {
            ServerConnection.Instance.Logout();
            MainViewModel.Instance.SwitchPage(this, nameof(Login));
        }

        #endregion

        #region Commands

        public RelayCommand ChallengeCommand => new RelayCommand((param) => Challenge(param as UserDto));

        public RelayCommand AcceptChallengeCommand => new RelayCommand((param) => AcceptChallenge(param as ChallengeDto));

        public RelayCommand CancelChallengeCommand => new RelayCommand((param) => CancelChallenge(param as ChallengeDto));

        public RelayCommand LogoutCommand => new RelayCommand((param) => Logout());

        #endregion
    }
}
