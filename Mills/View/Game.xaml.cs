using Mills.ViewModel;
using System.Windows.Controls;

namespace Mills.View
{
    public partial class Game : UserControl
    {
        public Game()
        {
            InitializeComponent();

            DataContext = GameViewModel.Instance;
        }
    }
}
