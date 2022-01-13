using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class LoginRequest : Request
    {
        public LoginRequest()
        {
            Method = RequestMethod.Login;
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
