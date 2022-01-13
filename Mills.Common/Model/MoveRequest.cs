using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class MoveRequest : Request
    {
        public MoveRequest()
        {
            Method = RequestMethod.Move;
        }

        public string SessionId { get; set; }

        public BoardPosition From { get; set; }

        public BoardPosition To { get; set; }
    }
}
