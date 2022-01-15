using Mills.Model;
using Mills.ViewModel;
using System.Windows;

namespace Mills
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel.Instance;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServerConnection.Instance.Logout();
        }
    }
}
