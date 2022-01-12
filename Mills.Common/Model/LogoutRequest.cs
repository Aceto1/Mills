using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class LogoutRequest : Request
    {
        public override RequestMethod Method => RequestMethod.Logout;

        public string SessionId { get; set; }
    }
}
