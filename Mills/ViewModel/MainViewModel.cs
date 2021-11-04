using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Mills.Enum;
using Mills.Model;
using Mills.ViewModel.Base;

namespace Mills.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Title = "Mills";
            ActivePlayer = 1;
            ActivePhase = 1;
            TokenPositions = new ObservableCollection<Tuple<ButtonPosition, int>>();
        }

        private string title;

        public string Title 
        { 
            get => title; 
            set 
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string ActivePlayerText => $"Spieler {activePlayer} ist an der Reihe";

        private int activePlayer;

        public int ActivePlayer
        {
            get => activePlayer;
            set { 
                activePlayer = value;
                OnPropertyChanged(nameof(ActivePlayer));
                OnPropertyChanged(nameof(ActivePlayerText));
            }
        }

        private int activePhase;

        public int ActivePhase
        {
            get => activePhase;
            set
            {
                activePhase = value;
                OnPropertyChanged(nameof(ActivePhase));
            }
        }

        public ObservableCollection<Tuple<ButtonPosition, int>> tokenPositions;

        public ObservableCollection<Tuple<ButtonPosition, int>> TokenPositions {
            get => tokenPositions;
            set
            {
                tokenPositions = value;
                OnPropertyChanged(nameof(TokenPositions));
            }
        }

        public void HandlePhaseOneClick(ButtonPosition position)
        {
            if (TokenPositions.Any(m => m.Item1 == position))
                return;

            TokenPositions.Add(new Tuple<ButtonPosition, int>(position, ActivePlayer));

            if (ActivePlayer == 1)
                ActivePlayer = 2;
            else
                ActivePlayer = 1;
        }

        public RelayCommand ButtonClickCommand => new((obj) => { 
            if(obj is not ButtonPosition btnPos)
                return;

            if(ActivePhase == 1)
            {
                HandlePhaseOneClick(btnPos);
                //TODO: 9 Steine pro Spieler, dann Phase wechseln
            }

            if (MillsManager.CheckForMill(tokenPositions.ToList())) {
                //TODO: Spieler muss einen Spielstein entfernen
            }

            OnPropertyChanged(nameof(TokenPositions));
        });

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
