using Mills.Enum;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mills.Model
{
    public class MillsManager
    {
        public static bool CheckForMill(ObservableDictionary<BoardPosition, PositionState> boardState)
        {
            return false;
        }

        public static BoardPosition[] GetAvailablePositions(ObservableDictionary<BoardPosition, PositionState> boardState, BoardPosition position)
        {
            var middlePositionRegex = new Regex("(.+Middle.+)|(.+Middle)");
            var stringPosition = position.ToString();
            var result = new List<BoardPosition>();

            if(middlePositionRegex.IsMatch(stringPosition))
            {

            } 
            else
            {

            }



            return new BoardPosition[] { BoardPosition.InnerTopLeft };
        }
    }
}
