using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class RegisterRequest : Request
    {
        public RegisterRequest()
        {
            Method = RequestMethod.Register;
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
