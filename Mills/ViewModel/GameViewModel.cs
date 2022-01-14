using Mills.Common.Enum;
using Mills.Common.Helper;
using Mills.Common.Model;
using Mills.Model;
using Mills.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Mills.ViewModel
{
    internal class GameViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Löst das PropertyChanged-Event mit dem angegebenen Propertynamen aus.
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Constructor

        private GameViewModel()
        {
            Phase = 1;
            ActivePlayer = PositionState.Player1;
            BoardState = new ObservableDictionary<BoardPosition, PositionState>();
        }

        #endregion

        #region Fields

        private PositionState activePlayer;

        private int phase;

        private BoardPosition selectedPosition;

        private bool remove;

        private string statusText;

        private ObservableDictionary<BoardPosition, PositionState> boardState;

        private static GameViewModel instance;

        #endregion

        #region Properties

        /// <summary>
        /// Formatierter String der anzeigt, welcher Spieler gerade dran ist
        /// </summary>
        public string ActivePlayerText => $"{(activePlayer == OwnColor ? "Sie" : "Ihr Gegner")} ist an der Reihe";

        /// <summary>
        /// Gerade aktiver Spieler.
        /// Spieler 1 = Weiß, Spieler 2 = Schwarz
        /// </summary>
        public PositionState ActivePlayer
        {
            get => activePlayer;
            set
            {
                activePlayer = value;
                OnPropertyChanged(nameof(ActivePlayer));
                OnPropertyChanged(nameof(ActivePlayerText));
            }
        }

        /// <summary>
        /// Aktive Spielphase. 
        /// 1 = Spieler platzieren abwechselnd Steine bis sie jeweils 9 platziert haben. 
        /// 2 = Spieler bewegen abwechselnd Steine bis ein Spieler nur noch 2 besitzt und verloren hat.
        /// </summary>
        public int Phase
        {
            get => phase;
            set
            {
                phase = value;
                OnPropertyChanged(nameof(Phase));
            }
        }

        /// <summary>
        /// Gibt an, ob der "Entfernen"-Modus aktiv ist. Das bedeutet das der nächste Klick auf einen Spielstein des gegenerischen Spielers diesen entfernt.
        /// </summary>
        public bool Remove
        {
            get => remove;
            set
            {
                if(!remove && value)
                {
                    StatusText = (ActivePlayer == OwnColor ? "Sie dürfen" : "Ihr Gegner darf") + " einen Stein entfernen.";
                }
                else if(!value && remove)
                {
                    StatusText = "";
                }

                remove = value;
                OnPropertyChanged(nameof(Remove));
                OnPropertyChanged(nameof(StatusText));
            }
        }

        /// <summary>
        /// Die zuletzt in Phase 2 ausgewählte Position eines eigenen Spielsteins. 
        /// </summary>
        public BoardPosition SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

        /// <summary>
        /// Anzeigetext, der Informationen über den Spielablauf enthält.
        /// </summary>
        public String StatusText
        {
            get => statusText;
            set
            {
                statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        /// <summary>
        /// Dictionary mit Schlüssel-Wert-Paaren die einer Position einen Wert (ob/welcher Spielstein sich darauf befindet, etc.) zuweist.
        /// </summary>
        public ObservableDictionary<BoardPosition, PositionState> BoardState
        {
            get => boardState;
            set
            {
                boardState = value;
                OnPropertyChanged(nameof(BoardState));
            }
        }

        public static GameViewModel Instance => instance ?? (instance = new GameViewModel());

        public DateTime LastAction = DateTime.MinValue;

        public PositionState OwnColor { get; set; }

        #endregion

        #region Methods

        public void SwitchPlayers()
        {
            if (ActivePlayer == PositionState.Player1)
                ActivePlayer = PositionState.Player2;
            else
                ActivePlayer = PositionState.Player1;
        }

        public void HandlePhaseOneClick(BoardPosition position)
        {
            // Feld mit Spielstein angeklickt = Nichts passiert
            if (BoardState.ContainsKey(position))
                return;

            ServerConnection.Instance.Place(position);
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

                ServerConnection.Instance.Move(SelectedPosition, position);
            }
            else if (value == (PositionState)ActivePlayer)
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
                var tokenCount = BoardState.Where(m => m.Value == OwnColor).Count();
                var availablePositions = MillsHelper.GetAvailablePositions(BoardState, position, tokenCount);

                // Und markieren
                foreach (var pos in availablePositions)
                {
                    BoardState.Add(pos, PositionState.AvailableForMove);
                }
            }
        }

        public void HandleRemoveClick(BoardPosition position)
        {
            //Leeres Feld angeklickt = Nichts passiert
            if (!BoardState.ContainsKey(position))
                return;

            ServerConnection.Instance.Remove(position);
        }

        public void Reset()
        {
            BoardState = new ObservableDictionary<BoardPosition, PositionState>();
            Phase = 1;
            ActivePlayer = PositionState.Player1;
            StatusText = "";
            Remove = false;
            LastAction = DateTime.MinValue;
        }

        public void Forfeit()
        {
            ServerConnection.Instance.Forfeit();
        }

        #endregion

        #region Commands

        /// <summary>
        /// Kommando welches ausgeführt wird, wenn ein Spielfeld angeklickt wird. Erhält die angeklickte Position als Parameter.
        /// </summary>
        public RelayCommand ButtonClickCommand => new((obj) =>
        {
            // Sind wir an der Reihe?
            if (ActivePlayer != OwnColor)
                return;

            if (obj is not BoardPosition btnPos)
                return;

            // "Debounce", damit man keine Aktionen spammen kann
            if (LastAction.AddSeconds(1) >= DateTime.UtcNow)
                return;

            // "Entfernen"-Modus aktiv?
            if (Remove)
                HandleRemoveClick(btnPos);
            else if (Phase == 1)
                HandlePhaseOneClick(btnPos);
            else
                HandlePhaseTwoClick(btnPos);

            LastAction = DateTime.UtcNow;

            OnPropertyChanged(nameof(BoardState));
        });

        public RelayCommand ForfeitButtonClickCommand => new RelayCommand((obj) => Forfeit());

        #endregion
    }
}
