using Mills.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Mills.View
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
            username.Focus();
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as LoginViewModel).Password = passwordBox.Password;
        }
    }
}
