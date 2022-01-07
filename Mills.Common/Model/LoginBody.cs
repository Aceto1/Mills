using Mills.Common.Interface;

namespace Mills.Common.Model
{
    public class LoginBody : RequestBody
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
