using Mills.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Mills.View
{
    public partial class Lobby : UserControl
    {
        public Lobby()
        {
            InitializeComponent();
            DataContext = LobbyViewModel.Instance;
        }
    }
}
