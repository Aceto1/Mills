using System;
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

        public MainViewModel()
        {
            // Die ersten Werte ohne zufälligen Seed sind oft nicht zufällig, daher werfen wir sie hier weg
            rnd.Next(1, 2);
            rnd.Next(1, 2);
            rnd.Next(1, 2);
            rnd.Next(1, 2);
            rnd.Next(1, 2);

            // Initialisierung der nötigen Poperties
            Title = "Mühle";
            ActivePlayer = rnd.Next(1, 2);
            ActivePhase = 1;
            BoardState = new ObservableDictionary<BoardPosition, PositionState>();
        }

        #endregion

        #region Fields

        private readonly Random rnd = new();

        private string title;

        private int activePlayer;

        private int activePhase;

        private int whiteTokenCount;

        private int blackTokenCount;

        private int tokensPlaced;

        private BoardPosition selectedPosition;

        private bool remove;

        private String statusText;

        private ObservableDictionary<BoardPosition, PositionState> boardState;

        #endregion

        #region Properties

        /// <summary>
        /// Titel des Fensters
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        /// <summary>
        /// Formatierter String der anzeigt, welcher Spieler gerade dran ist
        /// </summary>
        public string ActivePlayerText => $"{(activePlayer == 1 ? "Weiß" : "Schwarz")} ist an der Reihe";

        /// <summary>
        /// Gerade aktiver Spieler.
        /// Spieler 1 = Weiß, Spieler 2 = Schwarz
        /// </summary>
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

        /// <summary>
        /// Aktive Spielphase. 
        /// 1 = Spieler platzieren abwechselnd Steine bis sie jeweils 9 platziert haben. 
        /// 2 = Spieler bewegen abwechselnd Steine bis ein Spieler nur noch 2 beistzt und verloren hat.
        /// </summary>
        public int ActivePhase
        {
            get => activePhase;
            set
            {
                activePhase = value;
                OnPropertyChanged(nameof(ActivePhase));
            }
        }

        /// <summary>
        /// Anzahl der weißen Spielsteine auf dem Spielfeld.
        /// </summary>
        public int WhiteTokenCount
        {
            get => whiteTokenCount; set
            {
                whiteTokenCount = value;
                OnPropertyChanged(nameof(WhiteTokenCount));
            }
        }

        /// <summary>
        /// Anzahl der schwarzen Spielsteine auf dem Spielfeld.
        /// </summary>
        public int BlackTokenCount
        {
            get => blackTokenCount; set
            {
                blackTokenCount = value;
                OnPropertyChanged(nameof(BlackTokenCount));
            }
        }

        /// <summary>
        /// Anzahl der gesamten in Pahse 1 platztierten Spielsteine. Wechsel zu Pahse 2 wenn diese Variable den Wert 18 erreicht.
        /// </summary>
        public int TokensPlaced
        {
            get => tokensPlaced;
            set
            {
                tokensPlaced = value;
                OnPropertyChanged(nameof(TokensPlaced));
            }
        }

        /// <summary>
        /// Flag, ob der "Entfernen"-Modus aktiv ist. Das bedeutet das der nächste Klick auf einen Spielstein des gegenerischen Spielers diesen entfernt.
        /// </summary>
        public bool Remove
        {
            get => remove;
            set
            {
                remove = value;
                OnPropertyChanged(nameof(Remove));
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

        #endregion

        #region Methods

        public bool HandlePhaseOneClick(BoardPosition position)
        {
            // Feld mit Spielstein angeklickt = Nichts passiert
            if (BoardState.ContainsKey(position))
                return false;

            BoardState.Add(new KeyValuePair<BoardPosition, PositionState>(position, (PositionState)ActivePlayer));

            // Anzahl der platzierten Spielstein erhöhen
            TokensPlaced++;

            if (ActivePlayer == 1)
                WhiteTokenCount++;
            else
                BlackTokenCount++;

            // Phase 2 beginnt, wenn beide Spieler 9 Steine platziert haben
            if (TokensPlaced == 18)
            {
                ActivePhase++;
            }

            return true;
        }

        public bool HandlePhaseTwoClick(BoardPosition position)
        {
            //Leeres Feld angeklickt = Nichts passiert
            if (!BoardState.ContainsKey(position))
                return false;

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

                return true;
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
                var tokenCount = ActivePlayer == 1 ? WhiteTokenCount : BlackTokenCount;
                var availablePositions = MillsManager.GetAvailablePositions(BoardState, position, tokenCount);

                // Und markieren
                foreach (var pos in availablePositions)
                {
                    BoardState.Add(pos, PositionState.AvailableForMove);
                }
            }

            return false;
        }

        public bool HandleRemoveClick(BoardPosition position)
        {
            //Leeres Feld angeklickt = Nichts passiert
            if (!BoardState.ContainsKey(position))
                return false;

            // Überprüfen, ob der Wert der Posotion das Remove-Flag hat
            if (BoardState[position].HasFlag(PositionState.RemoveTarget))
            {
                BoardState.Remove(position);

                //Anzahl der Spielsteine für gegnerischen Spieler reudzieren
                if (ActivePlayer == 1)
                    BlackTokenCount--;
                else
                    WhiteTokenCount--;

                // Flag auf restlichen Positionen entfernen
                foreach (var pos in BoardState)
                {
                    if (pos.Value.HasFlag(PositionState.RemoveTarget))
                        BoardState[pos.Key] = pos.Value & ~PositionState.RemoveTarget;
                }

                // "Entfernen" Modus verlassen
                Remove = false;
                StatusText = "";

                return true;
            }

            return false;
        }

        /// <summary>
        /// Setzt das Spiel zurück und startet ein neues
        /// </summary>
        public void Reset()
        {
            BoardState = new ObservableDictionary<BoardPosition, PositionState>();
            ActivePhase = 1;
            ActivePlayer = rnd.Next(1, 2);
            TokensPlaced = 0;
            WhiteTokenCount = 0;
            BlackTokenCount = 0;
            SelectedPosition = BoardPosition.None;
            Remove = false;
            StatusText = "";
        }

        /// <summary>
        /// Markiert alle Spielsteine mit einem entsprechenden Flag, wenn sie entfernt werden können.
        /// Dabei müssen die jeweiligen Spielsteine folgende Bedinungen erfüllen:
        /// 1. Die Spielsteine müssen dem generischen Spieler gehören
        /// 2. Die Spielsteine dürfen sich nicht in einer Mühle befinden
        /// 3. Sollte der gegnersiche Spieler nur Mühlen auf dem Feld besitzen, darf ein beliebiger seiner Steine entfernt werden
        /// </summary>
        public void SetRemoveTargets()
        {
            var validTargets = new List<BoardPosition>();

            // Alle Positionen des Speilbretts durchlaufen
            foreach (var position in BoardState)
            {
                // Überprüfen das
                // 1. An der aktuellen Stelle kein eigener Spielstein ist
                // 2. Die aktuelle Stelle nicht Teil einer Mühle ist
                if (position.Value != (PositionState)ActivePlayer &&
                    !MillsManager.CheckForMill(BoardState, position.Key, (int)position.Value))
                {
                    validTargets.Add(position.Key);
                }
            }

            // Sollten sich aus dem vorherigen Durchlauf keine validen Spielsteine ergeben haben (weil alle gegnerischen Teil einer Mühle sind)
            // werden alle Spielsteine des Gegners als valide markiert
            if (validTargets.Count == 0)
            {
                foreach (var position in BoardState)
                {
                    if (position.Value != (PositionState)ActivePlayer)
                    {
                        BoardState[position.Key] |= PositionState.RemoveTarget;
                    }
                }
            }
            // Ansonsten werden die Spielsteine aus dem vorherigen Durchlauf markiert
            else
            {
                foreach (var position in validTargets)
                {
                    BoardState[position] |= PositionState.RemoveTarget;
                }
            }

            // Den "Entfernen"-Modus aktivieren
            Remove = true;
            StatusText = "Eine Mühle! Entfernen Sie einen Spielstein Ihres Gegeners.";
        }

        #endregion

        #region Commands

        /// <summary>
        /// Kommando welches ausgeführt wird, wenn ein Spielfeld angeklickt wird. Erhält die angeklickte Position als Parameter.
        /// </summary>
        public RelayCommand ButtonClickCommand => new((obj) =>
        {
            if (obj is not BoardPosition btnPos)
                return;

            if (ActivePhase != 1 && (WhiteTokenCount <= 2 || BlackTokenCount <= 2))
                return;

            bool moveMade;

            if (Remove)
                moveMade = HandleRemoveClick(btnPos);
            else if (ActivePhase == 1)
                moveMade = HandlePhaseOneClick(btnPos);
            else
                moveMade = HandlePhaseTwoClick(btnPos);

            if (moveMade && MillsManager.CheckForMill(BoardState, btnPos, ActivePlayer))
                SetRemoveTargets();

            if (ActivePhase != 1)
            {
                if (BlackTokenCount <= 2)
                {
                    StatusText = "Weiß hat gewonnen! Drücken sie \"Neu Starten\" um ein neues Spiel zu starten!";
                }
                else if (WhiteTokenCount <= 2)
                {
                    StatusText = "Schwarz hat gewonnen! Drücken sie \"Neu Starten\" um ein neues Spiel zu starten!";
                }
            }

            if (moveMade && !Remove)
            {
                // Spielerwechsel
                if (ActivePlayer == 1)
                    ActivePlayer = 2;
                else
                    ActivePlayer = 1;
            }

            OnPropertyChanged(nameof(BoardState));
        });

        /// <summary>
        /// Kommando welches ausgeführt wird, wenn der "Neu Starten" Knopf angeklickt wird.
        /// </summary>
        public RelayCommand ResetCommand => new((obj) => Reset());

        #endregion
    }
}
