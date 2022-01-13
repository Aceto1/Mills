using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class LogoutRequest : Request
    {
        public LogoutRequest()
        {
            Method = RequestMethod.Logout;
        }

        public string SessionId { get; set; }
    }
}
