using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ForfeitRequest : Request
    {
        public ForfeitRequest()
        {
            Method = RequestMethod.Forfeit;
        }

        public string SessionId { get; set; }
    }
}
