using Mills.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Mills.View
{
    /// <summary>
    /// Interaktionslogik für Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as RegisterViewModel).Password = password.Password;
        }

        private void passwordRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as RegisterViewModel).PasswordRepeat = passwordRepeat.Password;
        }
    }
}
