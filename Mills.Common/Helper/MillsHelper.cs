using Mills.Common.Enum;
using Mills.Common.Model;
using System.Collections.Generic;
using System.Linq;

namespace Mills.Common.Helper
{
    public class MillsHelper
    {
        /// <summary>
        /// Sucht nach einer Mühle des angegeben Spielers, die die angegebene Position auf dem Spielbrett beinhaltet.
        /// </summary>
        /// <param name="boardState">Aktuelles Spielbrett</param>
        /// <param name="position">Position, die überprüft werden soll</param>
        /// <param name="player">Spieler, für den überprüft werden soll</param>
        /// <returns>Ob die angegebene Position Teil einer Mühle ist.</returns>
        public static bool CheckForMill(ObservableDictionary<BoardPosition, PositionState> boardState, BoardPosition position, PositionState player)
        {
            var stringPosition = position.ToString();
            var positionState = boardState[position];

            if (positionState != player)
                return false;

            if (stringPosition.StartsWith("Outer"))
            {
                if (stringPosition.StartsWith("OuterTop"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.OuterTopMiddle] == positionState && boardState[BoardPosition.OuterTopRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterMiddleLeft] == positionState && boardState[BoardPosition.OuterBottomLeft] == positionState)
                            return true;
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        if (boardState[BoardPosition.OuterTopRight] == positionState && boardState[BoardPosition.OuterTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleTopMiddle] == positionState && boardState[BoardPosition.InnerTopMiddle] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.OuterTopMiddle] == positionState && boardState[BoardPosition.OuterTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterMiddleRight] == positionState && boardState[BoardPosition.OuterBottomRight] == positionState)
                            return true;
                    }
                }
                else if (stringPosition.StartsWith("OuterMiddle"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.MiddleMiddleLeft] == positionState && boardState[BoardPosition.InnerMiddleLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterTopLeft] == positionState && boardState[BoardPosition.OuterBottomLeft] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.MiddleMiddleRight] == positionState && boardState[BoardPosition.InnerMiddleRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterTopRight] == positionState && boardState[BoardPosition.OuterBottomRight] == positionState)
                            return true;
                    }
                }
                else
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.OuterMiddleLeft] == positionState && boardState[BoardPosition.OuterTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterBottomMiddle] == positionState && boardState[BoardPosition.OuterBottomRight] == positionState)
                            return true;
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        if (boardState[BoardPosition.MiddleBottomMiddle] == positionState && boardState[BoardPosition.InnerBottomMiddle] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterBottomRight] == positionState && boardState[BoardPosition.OuterBottomLeft] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.OuterMiddleRight] == positionState && boardState[BoardPosition.OuterTopRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterBottomMiddle] == positionState && boardState[BoardPosition.OuterBottomLeft] == positionState)
                            return true;
                    }
                }
            }
            else if (stringPosition.StartsWith("Middle"))
            {
                if (stringPosition.StartsWith("MiddleTop"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.MiddleTopMiddle] == positionState && boardState[BoardPosition.MiddleTopRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleMiddleLeft] == positionState && boardState[BoardPosition.MiddleBottomLeft] == positionState)
                            return true;
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        if (boardState[BoardPosition.MiddleTopRight] == positionState && boardState[BoardPosition.MiddleTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterTopMiddle] == positionState && boardState[BoardPosition.InnerTopMiddle] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.MiddleTopMiddle] == positionState && boardState[BoardPosition.MiddleTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleMiddleRight] == positionState && boardState[BoardPosition.MiddleBottomRight] == positionState)
                            return true;
                    }
                }
                else if (stringPosition.StartsWith("MiddleMiddle"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.OuterMiddleLeft] == positionState && boardState[BoardPosition.InnerMiddleLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleTopLeft] == positionState && boardState[BoardPosition.MiddleBottomLeft] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.OuterMiddleRight] == positionState && boardState[BoardPosition.InnerMiddleRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleTopRight] == positionState && boardState[BoardPosition.MiddleBottomRight] == positionState)
                            return true;
                    }
                }
                else
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.MiddleMiddleLeft] == positionState && boardState[BoardPosition.MiddleTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleBottomMiddle] == positionState && boardState[BoardPosition.MiddleBottomRight] == positionState)
                            return true;
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        if (boardState[BoardPosition.OuterBottomMiddle] == positionState && boardState[BoardPosition.InnerBottomMiddle] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleBottomRight] == positionState && boardState[BoardPosition.MiddleBottomLeft] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.MiddleMiddleRight] == positionState && boardState[BoardPosition.MiddleTopRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.MiddleBottomMiddle] == positionState && boardState[BoardPosition.MiddleBottomLeft] == positionState)
                            return true;
                    }
                }
            }
            else
            {
                if (stringPosition.StartsWith("InnerTop"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.InnerTopMiddle] == positionState && boardState[BoardPosition.InnerTopRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerMiddleLeft] == positionState && boardState[BoardPosition.InnerBottomLeft] == positionState)
                            return true;
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        if (boardState[BoardPosition.InnerTopRight] == positionState && boardState[BoardPosition.InnerTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.OuterTopMiddle] == positionState && boardState[BoardPosition.MiddleTopMiddle] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.InnerTopMiddle] == positionState && boardState[BoardPosition.InnerTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerMiddleRight] == positionState && boardState[BoardPosition.InnerBottomRight] == positionState)
                            return true;
                    }
                }
                else if (stringPosition.StartsWith("InnerMiddle"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.MiddleMiddleLeft] == positionState && boardState[BoardPosition.OuterMiddleLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerTopLeft] == positionState && boardState[BoardPosition.InnerBottomLeft] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.MiddleMiddleRight] == positionState && boardState[BoardPosition.OuterMiddleRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerTopRight] == positionState && boardState[BoardPosition.InnerBottomRight] == positionState)
                            return true;
                    }
                }
                else
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        if (boardState[BoardPosition.InnerMiddleLeft] == positionState && boardState[BoardPosition.InnerTopLeft] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerBottomMiddle] == positionState && boardState[BoardPosition.InnerBottomRight] == positionState)
                            return true;
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        if (boardState[BoardPosition.MiddleBottomMiddle] == positionState && boardState[BoardPosition.OuterBottomMiddle] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerBottomRight] == positionState && boardState[BoardPosition.InnerBottomLeft] == positionState)
                            return true;
                    }
                    else
                    {
                        if (boardState[BoardPosition.InnerMiddleRight] == positionState && boardState[BoardPosition.InnerTopRight] == positionState)
                            return true;

                        if (boardState[BoardPosition.InnerBottomMiddle] == positionState && boardState[BoardPosition.InnerBottomLeft] == positionState)
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Sucht die Positionen, auf die ein ausgewählter Spielstein verschoben werden darf.
        /// </summary>
        /// <param name="boardState">Aktuelles Spielbrett</param>
        /// <param name="position">Position, die überprüft werden soll</param>
        /// <param name="tokensLeft">Anzahl der Spielsteine, die der Spieler besitzt</param>
        /// <returns>Liste der Positionen, auf die der Spielstein verschoben werden darf.</returns>
        public static BoardPosition[] GetAvailablePositions(ObservableDictionary<BoardPosition, PositionState> boardState, BoardPosition position, int tokensLeft)
        {
            var stringPosition = position.ToString();
            var moveList = new List<BoardPosition>();

            // 3 oder weniger Spielstein übrig, also darf der Spieler zu jedem freien Feld springen
            if (tokensLeft <= 3)
            {
                var enumList = System.Enum.GetValues(typeof(BoardPosition)).Cast<BoardPosition>();

                foreach (var enumValue in enumList)
                {
                    if (!boardState.ContainsKey(enumValue))
                    {
                        moveList.Add(enumValue);
                    }
                }

                return moveList.ToArray();
            }

            // 4 oder mehr Spielsteine übrig, also darf der Spieler nur zu freien, benachrbarten Feldern ziehen
            if (stringPosition.StartsWith("Outer"))
            {
                if (stringPosition.StartsWith("OuterTop"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.OuterMiddleLeft);
                        moveList.Add(BoardPosition.OuterTopMiddle);
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        moveList.Add(BoardPosition.OuterTopLeft);
                        moveList.Add(BoardPosition.OuterTopRight);
                        moveList.Add(BoardPosition.MiddleTopMiddle);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.OuterMiddleRight);
                        moveList.Add(BoardPosition.OuterTopMiddle);
                    }
                }
                else if (stringPosition.StartsWith("OuterMiddle"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.MiddleMiddleLeft);
                        moveList.Add(BoardPosition.OuterTopLeft);
                        moveList.Add(BoardPosition.OuterBottomLeft);

                    }
                    else
                    {
                        moveList.Add(BoardPosition.MiddleMiddleRight);
                        moveList.Add(BoardPosition.OuterTopRight);
                        moveList.Add(BoardPosition.OuterBottomRight);
                    }
                }
                else
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.OuterMiddleLeft);
                        moveList.Add(BoardPosition.OuterBottomMiddle);
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        moveList.Add(BoardPosition.OuterBottomLeft);
                        moveList.Add(BoardPosition.OuterBottomRight);
                        moveList.Add(BoardPosition.MiddleTopMiddle);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.OuterMiddleRight);
                        moveList.Add(BoardPosition.OuterBottomMiddle);
                    }
                }
            }
            else if (stringPosition.StartsWith("Middle"))
            {
                if (stringPosition.StartsWith("MiddleTop"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.MiddleMiddleLeft);
                        moveList.Add(BoardPosition.MiddleTopMiddle);
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        moveList.Add(BoardPosition.MiddleTopLeft);
                        moveList.Add(BoardPosition.MiddleTopRight);
                        moveList.Add(BoardPosition.InnerTopMiddle);
                        moveList.Add(BoardPosition.OuterTopMiddle);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.MiddleMiddleRight);
                        moveList.Add(BoardPosition.MiddleTopMiddle);
                    }
                }
                else if (stringPosition.StartsWith("MiddleMiddle"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.OuterMiddleLeft);
                        moveList.Add(BoardPosition.InnerMiddleLeft);
                        moveList.Add(BoardPosition.MiddleTopLeft);
                        moveList.Add(BoardPosition.MiddleBottomLeft);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.OuterMiddleRight);
                        moveList.Add(BoardPosition.InnerMiddleRight);
                        moveList.Add(BoardPosition.MiddleTopRight);
                        moveList.Add(BoardPosition.MiddleBottomRight);
                    }
                }
                else
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.MiddleMiddleLeft);
                        moveList.Add(BoardPosition.MiddleBottomMiddle);
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        moveList.Add(BoardPosition.MiddleBottomLeft);
                        moveList.Add(BoardPosition.MiddleBottomRight);
                        moveList.Add(BoardPosition.InnerBottomMiddle);
                        moveList.Add(BoardPosition.OuterBottomMiddle);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.MiddleMiddleRight);
                        moveList.Add(BoardPosition.MiddleBottomMiddle);
                    }
                }
            }
            else
            {
                if (stringPosition.StartsWith("InnerTop"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.InnerMiddleLeft);
                        moveList.Add(BoardPosition.InnerTopMiddle);
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        moveList.Add(BoardPosition.InnerTopLeft);
                        moveList.Add(BoardPosition.InnerTopRight);
                        moveList.Add(BoardPosition.MiddleTopMiddle);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.InnerMiddleRight);
                        moveList.Add(BoardPosition.InnerTopMiddle);
                    }
                }
                else if (stringPosition.StartsWith("InnerMiddle"))
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.MiddleMiddleLeft);
                        moveList.Add(BoardPosition.InnerTopLeft);
                        moveList.Add(BoardPosition.InnerBottomLeft);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.MiddleMiddleRight);
                        moveList.Add(BoardPosition.InnerTopRight);
                        moveList.Add(BoardPosition.InnerBottomRight);
                    }
                }
                else
                {
                    if (stringPosition.EndsWith("Left"))
                    {
                        moveList.Add(BoardPosition.InnerMiddleLeft);
                        moveList.Add(BoardPosition.InnerBottomMiddle);
                    }
                    else if (stringPosition.EndsWith("Middle"))
                    {
                        moveList.Add(BoardPosition.InnerBottomLeft);
                        moveList.Add(BoardPosition.InnerBottomRight);
                        moveList.Add(BoardPosition.MiddleBottomMiddle);
                    }
                    else
                    {
                        moveList.Add(BoardPosition.InnerMiddleRight);
                        moveList.Add(BoardPosition.InnerBottomMiddle);
                    }
                }
            }

            var result = new List<BoardPosition>();

            // Felder rausfiltern, auf denen bereits ein Spielstein steht
            foreach (var item in moveList)
            {
                if (!boardState.TryGetValue(item, out var value) || value == PositionState.AvailableForMove)
                {
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Überprüft, ob der gewählte Spieler noch Möglichkeiten hat, seine Steine zu verschieben.
        /// </summary>
        /// <param name="boardState">Aktuelles Spielbrett</param>
        /// <param name="tokensLeft">Anzahl der Spielsteine, die der Spieler besitzt</param>
        /// <param name="player">Spieler, für den überprüft werden soll</param>
        /// <returns>Ob der Spieler noch Zugmöglichkeiten hat.</returns>
        public static bool HasAvailableMoves(ObservableDictionary<BoardPosition, PositionState> boardState, int tokensLeft, PositionState player)
        {
            foreach (var position in boardState)
            {
                if (position.Value == player &&
                    GetAvailablePositions(boardState, position.Key, tokensLeft).Length != 0)
                    return true;
            }

            return false;
        }

        public static void SetRemoveTargets(ObservableDictionary<BoardPosition, PositionState> boardState, PositionState activePlayer)
        {
            var validTargets = new List<BoardPosition>();

            // Alle Positionen des Spielbretts durchlaufen
            foreach (var position in boardState)
            {
                // Überprüfen das
                // 1. An der aktuellen Stelle kein eigener Spielstein ist
                // 2. Die aktuelle Stelle nicht Teil einer Mühle ist
                if (position.Value != (PositionState)activePlayer &&
                    !MillsHelper.CheckForMill(boardState, position.Key, position.Value))
                {
                    validTargets.Add(position.Key);
                }
            }

            // Sollten sich aus dem vorherigen Durchlauf keine validen Spielsteine ergeben haben (weil alle gegnerischen Teil einer Mühle sind)
            // werden alle Spielsteine des Gegners als valide markiert
            if (validTargets.Count == 0)
            {
                foreach (var position in boardState)
                {
                    if (position.Value != (PositionState)activePlayer)
                    {
                        boardState[position.Key] |= PositionState.RemoveTarget;
                    }
                }
            }
            // Ansonsten werden die Spielsteine aus dem vorherigen Durchlauf markiert
            else
            {
                foreach (var position in validTargets)
                {
                    boardState[position] |= PositionState.RemoveTarget;
                }
            }
        }
    }
}
