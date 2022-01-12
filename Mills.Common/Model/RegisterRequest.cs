using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class RegisterRequest : Request
    {
        public override RequestMethod Method => RequestMethod.Register;

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
