using Mills.Database.Entities.Base;

namespace Mills.Database.Entities.User
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string MailAddress { get; set; }
    }
}
