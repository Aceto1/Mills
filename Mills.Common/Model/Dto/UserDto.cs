using Mills.Common.Enum;

namespace Mills.Common.Model.Dto
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public UserStatus Status { get; set; }
    }
}
