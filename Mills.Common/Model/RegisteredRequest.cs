using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class RegisteredRequest : Request
    {
        public RegisteredRequest()
        {
            Method = RequestMethod.Registered;
        }
    }
}
