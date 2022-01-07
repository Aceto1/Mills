using Mills.Model;
using Mills.View;
using Mills.ViewModel.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mills.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
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

        private string username;

        private string password;

        #endregion

        #region Properties

        public string Username
        {
            get => username;
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        #endregion

        #region Methods

        private void Login()
        {
            var result = ServerConnection.Login(username, password);

            if(result)
                PageSwitcher.Instance.SwitchPage(this, nameof(Game));
        }

        private void Register()
        {
            PageSwitcher.Instance.SwitchPage(this, nameof(View.Register));
        }

        #endregion

        #region Commands

        public RelayCommand LoginCommand => new((obj) => Login(), (obj) => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password));

        public RelayCommand RegisterCommand => new((obj) => Register());
        #endregion
    }
}
