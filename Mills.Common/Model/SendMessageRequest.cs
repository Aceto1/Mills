using Mills.Common.Enum;
using Mills.Common.Model.Dto;

namespace Mills.Common.Model
{
    public class SendMessageRequest : Request
    {
        public SendMessageRequest()
        {
            Method = RequestMethod.SendMessage;
        }

        public UserDto FromUser { get; set; }

        public string Message { get; set; }
    }
}
