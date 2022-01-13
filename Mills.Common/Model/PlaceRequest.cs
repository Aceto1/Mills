using Mills.Common.Enum;
using Mills.Common.Model.Dto;

namespace Mills.Common.Model
{
    public class PlaceRequest : Request
    {
        public PlaceRequest()
        {
            Method = RequestMethod.Place;
        }

        public string SessionId { get; set; }

        public UserDto Player { get; set; }

        public BoardPosition Position { get; set; }
    }
}
