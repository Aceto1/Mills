using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class PlacedRequest : Request
    {
        public PlacedRequest()
        {
            Method = RequestMethod.Placed;
        }

        public BoardPosition Position { get; set; }

        public bool Remove { get; set; }

        public bool PhaseChange { get; set; }
    }
}
