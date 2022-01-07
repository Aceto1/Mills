using Mills.Database.Entities.Base;

namespace Mills.Database.Entities.Game
{
    public class GameResult : BaseEntity
    {
        public int UserId1 { get; set; }

        public int UserId2 { get; set; }

        public int WinnerId { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
