using Mills.Common.Enum;
using Mills.Common.Model.Dto;

namespace Mills.Common.Model
{
    public class LoggedInRequest : Request
    {
        public LoggedInRequest()
        {
            Method = RequestMethod.LoggedIn;
        }

        public UserDto User { get; set; }

        public string SessionId { get; set; }
    }
}
