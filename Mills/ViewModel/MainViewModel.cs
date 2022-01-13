using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Mills.Common.Enum;
using Mills.Common.Model.Dto;
using Mills.View;
using Mills.ViewModel.Base;

namespace Mills.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
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

        #region Constructor

        private MainViewModel()
        {
            PageSwitcher.Instance.PageChangeRequested += (sender, pageName) =>
            {
                SwitchPage(sender, pageName.Pagename);
            };

            PageSwitcher.Instance.SwitchPage(this, nameof(Login));
        }

        #endregion

        #region Fields

        private readonly Dictionary<string, ContentControl> pages = new()
        {
            { nameof(Login), new Login() },
            { nameof(Game), new Game() },
            { nameof(Lobby), new Lobby() },
            { nameof(Register), new Register() }
        };

        private ContentControl content;

        private static MainViewModel instance;

        private UserDto currentUser;

        #endregion

        #region Properties

        /// <summary>
        /// Der dynamische Inhalt des Fensters
        /// </summary>
        public ContentControl Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        /// <summary>
        /// Titel des Fensters
        /// </summary>
        public string Title => "Mühle" + CurrentUser == null ? $" - {CurrentUser.Username}" : "";

        public UserDto CurrentUser 
        { 
            get => currentUser; 
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
                OnPropertyChanged(nameof(Title));
            }
        }

        public static MainViewModel Instance => instance ?? (instance = new MainViewModel());

        #endregion

        #region Methods

        public void SwitchPage(object sender, string pageName)
        {
            if (pages.TryGetValue(pageName, out var page))
                Content = page;
        }

        public void ShowMessage(string message, Severity severity)
        {
            string caption = "";
            MessageBoxImage icon = MessageBoxImage.None;

            switch (severity)
            {
                case Severity.Information:
                    caption = "Information";
                    icon = MessageBoxImage.Information;
                    break;
                case Severity.Warning:
                    caption = "Warnung";
                    icon = MessageBoxImage.Warning;
                    break;
                case Severity.Error:
                    caption = "Fehler";
                    icon = MessageBoxImage.Error;
                    break;
                default:
                    break;
            }

            MessageBox.Show(message, caption, MessageBoxButton.OK, icon);
        }

        #endregion
    }
}
