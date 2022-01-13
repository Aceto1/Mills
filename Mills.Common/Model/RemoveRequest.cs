using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class RemoveRequest : Request
    {
        public RemoveRequest()
        {
            Method = RequestMethod.Remove;
        }

        public string SessionId { get; set; }

        public BoardPosition Position { get; set; }
    }
}
