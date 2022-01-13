using Mills.Common.Enum;
using Mills.Common.Model.Dto;

namespace Mills.Common.Model
{
    public class SendActiveUsersRequest : Request
    {
        public SendActiveUsersRequest()
        {
            Method = RequestMethod.SendActiveUsers;
        }

        public UserDto[] Users { get; set; }
    }
}
