using Mills.Common.Enum;
using Mills.Common.Model.Dto;

namespace Mills.Common.Model
{
    public class GameStartedRequest : Request
    {
        public GameStartedRequest()
        {
            Method = RequestMethod.GameStarted;
        }

        public UserDto AgainstUser { get; set; }

        public bool Starting { get; set; }
    }
}
