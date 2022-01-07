using Mills.Common.Enum;
using System.Collections.Generic;

namespace Mills.Server.Model
{
    public class Game
    {
        public bool Remove { get; set; }

        public int ActiveUserId { get; set; }

        public int UserId1 { get; set; }

        public int UserId2 { get; set; }

        public Dictionary<BoardPosition, PositionState> BoardState { get; set; }
    }
}
