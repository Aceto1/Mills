using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class RemovedRequest : Request
    {
        public RemovedRequest()
        {
            Method = RequestMethod.Removed;
        }

        public BoardPosition Position { get; set; }
    }
}
