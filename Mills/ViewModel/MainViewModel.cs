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

        private int whiteTokenCount;

        public int WhiteTokenCount
        {
            get => whiteTokenCount; set
            {
                whiteTokenCount = value;
                OnPropertyChanged(nameof(WhiteTokenCount));
            }
        }

        private int blackTokenCount;

        public int BlackTokenCount
        {
            get => blackTokenCount; set
            {
                blackTokenCount = value;
                OnPropertyChanged(nameof(BlackTokenCount));
            }
        }

        private int tokensPlaced;

        public int TokensPlaced
        {
            get => tokensPlaced;
            set
            {
                tokensPlaced = value;
                OnPropertyChanged(nameof(TokensPlaced));
            }
        }

        public BoardPosition SelectedPosition
        {
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
            // Feld mit Spielstein angeklickt = Nichts passiert
            if (BoardState.ContainsKey(position))
                return;

            BoardState.Add(new KeyValuePair<BoardPosition, PositionState>(position, (PositionState)ActivePlayer));

            // Anzahl der platzierten Spielstein erhöhen
            TokensPlaced++;

            // Phase 2 beginnt, wenn beide Spieler 9 Steine platziert haben
            if (TokensPlaced == 18)
            {
                ActivePhase++;
            }
        }

        public void HandlePhaseTwoClick(BoardPosition position)
        {
            //Leeres Feld angeklickt = Nichts passiert
            if (!BoardState.ContainsKey(position))
                return;

            var value = BoardState[position];

            if (value == PositionState.AvailableForMove)
            {
                // Verfügbares Feld selektiert, also erst alle als verfügbar markierten Felder löschen
                foreach (var pos in BoardState)
                {
                    if (pos.Value == PositionState.AvailableForMove)
                        BoardState.Remove(pos);
                };

                // Vorher selektierten Spielstein auf gerade ausgewählt Position verschieben und alte Position löschen
                BoardState.Add(position, BoardState[selectedPosition]);
                BoardState.Remove(selectedPosition);
            }
            else
            {
                //Neuer Stein selektiert, also erst alle bisher als verfügbar markierten Felder löschen
                foreach (var pos in BoardState)
                {
                    if (pos.Value == PositionState.AvailableForMove)
                        BoardState.Remove(pos);
                }

                // Selektierten Spielstein merken
                SelectedPosition = position;

                // Mögliche Felder raussuchen
                var tokenCount = ActivePlayer == 1 ? WhiteTokenCount : BlackTokenCount;
                var availablePositions = MillsManager.GetAvailablePositions(BoardState, position, tokenCount);

                // Und markieren
                foreach (var pos in availablePositions)
                {
                    BoardState.Add(pos, PositionState.AvailableForMove);
                }
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
                // TODO: Aktiver Spieler muss Stein des Gegners auswählen und entfernen
            }

            // Spielerwechsel
            if (ActivePlayer == 1)
            {
                ActivePlayer = 2;
            }
            else
            {
                ActivePlayer = 1;
            }

            OnPropertyChanged(nameof(BoardState));
        });

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
