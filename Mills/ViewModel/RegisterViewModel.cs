using Mills.Model;
using Mills.ViewModel.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mills.ViewModel
{
    public class RegisterViewModel : INotifyPropertyChanged
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

        private string passwordRepeat;

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

        public string PasswordRepeat
        {
            get => passwordRepeat;
            set
            {
                passwordRepeat = value;
                OnPropertyChanged(nameof(PasswordRepeat));
            }
        }

        #endregion

        #region Methods

        private void Login()
        {
            PageSwitcher.Instance.SwitchPage(this, nameof(View.Login));
        }

        private void Register()
        {
            if (ServerConnection.Instance.Register(username, password))
                PageSwitcher.Instance.SwitchPage(this, nameof(View.Login));
        }

        #endregion

        #region Commands

        public RelayCommand RegisterCommand => new((obj) => Register(), 
            (obj) => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(PasswordRepeat) && Password == PasswordRepeat);

        public RelayCommand LoginCommand => new((obj) => Login());

        #endregion
    }
}
