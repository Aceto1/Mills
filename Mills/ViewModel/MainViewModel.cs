using System.Collections.Generic;
using System.ComponentModel;
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
            BoardState = new ObservableDictionary<BoardPosition, PositionState>();
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
            set
            {
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

        private int whitePlacedTokens;

        public int WhitePlacedTokens
        {
            get => whitePlacedTokens; set
            {
                whitePlacedTokens = value;
                OnPropertyChanged(nameof(WhitePlacedTokens));
            }
        }

        private int blackPlacedTokens;

        public int BlackPlacedTokens
        {
            get => blackPlacedTokens; set
            {
                blackPlacedTokens = value;
                OnPropertyChanged(nameof(BlackPlacedTokens));
            }
        }

        public BoardPosition SelectedPosition { 
            get => selectedPosition; 
            set
            {
                selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        private BoardPosition selectedPosition;

        public ObservableDictionary<BoardPosition, PositionState> boardState;

        public ObservableDictionary<BoardPosition, PositionState> BoardState
        {
            get => boardState;
            set
            {
                boardState = value;
                OnPropertyChanged(nameof(BoardState));
            }
        }

        public void HandlePhaseOneClick(BoardPosition position)
        {
            if (BoardState.ContainsKey(position))
                return;

            BoardState.Add(new KeyValuePair<BoardPosition, PositionState>(position, (PositionState)ActivePlayer));

            if (ActivePlayer == 1)
            {
                WhitePlacedTokens++;
                ActivePlayer = 2;
            }
            else
            {
                BlackPlacedTokens++;
                ActivePlayer = 1;
            }

            if (WhitePlacedTokens == 9 && BlackPlacedTokens == 9)
            {
                ActivePhase++;
            }
        }

        public void HandlePhaseTwoClick(BoardPosition position)
        {
            if (!BoardState.ContainsKey(position))
                return;

            var value = BoardState[position];

            if (value == PositionState.AvailableForMove)
            {
                var enumerator = BoardState.GetEnumerator();
                var toRemove = new List<BoardPosition>();
                do
                {
                    if (enumerator.Current.Value == PositionState.AvailableForMove)
                    {
                        toRemove.Add(enumerator.Current.Key);
                    }
                }
                while (enumerator.MoveNext());

                foreach (var item in toRemove)
                {
                    BoardState.Remove(item);
                }

                BoardState.Add(position, BoardState[selectedPosition]);
                BoardState.Remove(selectedPosition);
            } else
            {
                SelectedPosition = position;

                var availablePositions = MillsManager.GetAvailablePositions(BoardState, position);
                foreach (var pos in availablePositions)
                {
                    BoardState.Add(pos, PositionState.AvailableForMove);
                }
            }       

            if (ActivePlayer == 1)
            {
                ActivePlayer = 2;
            }
            else
            {
                ActivePlayer = 1;
            }
        }

        public RelayCommand ButtonClickCommand => new((obj) =>
        {
            if (obj is not BoardPosition btnPos)
                return;

            if (ActivePhase == 1)
            {
                HandlePhaseOneClick(btnPos);
            }
            else if (ActivePhase == 2)
            {
                HandlePhaseTwoClick(btnPos);
            }

            if (MillsManager.CheckForMill(BoardState))
            {
                //TODO: Spieler muss einen Spielstein entfernen
            }

            OnPropertyChanged(nameof(BoardState));
        });

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
