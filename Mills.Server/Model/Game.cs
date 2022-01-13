using Mills.Common.Enum;
using Mills.Common.Model;
using System.Collections.Generic;

namespace Mills.Server.Model
{
    public class Game
    {
        public Game()
        {
            BoardState = new ObservableDictionary<BoardPosition, PositionState>();
            Phase = 1;
        }
        public bool Remove { get; set; }

        public int ActiveUserId { get; set; }

        public int UserId1 { get; set; }

        public int UserId2 { get; set; }

        public ObservableDictionary<BoardPosition, PositionState> BoardState { get; set; }

        public int Phase { get; set; }

        public int TokensPlaced { get; set; }

        public int User1TokenCount { get; set; }

        public int User2TokenCount { get; set; }

        public void SwitchActivePlayer()
        {
            if(ActiveUserId == UserId1)
                ActiveUserId = UserId2;
            else
                ActiveUserId = UserId1;
        }
    }
}