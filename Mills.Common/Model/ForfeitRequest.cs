using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ForfeitRequest : Request
    {
        public ForfeitRequest()
        {
            Method = RequestMethod.Forfeit;
        }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }

        public string SessionId { get; set; }
    }
}
