using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class MovedRequest : Request
    {
        public MovedRequest()
        {
            Method = RequestMethod.Moved;
        }

        public BoardPosition From { get; set; }

        public BoardPosition To { get; set; }

        public bool Remove { get; set; }
    }
}
