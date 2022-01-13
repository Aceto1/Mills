using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ChallengeCancelledRequest : Request
    {
        public ChallengeCancelledRequest()
        {
            Method = RequestMethod.ChallengeCancelled;
        }

        public string SessionId { get; set; }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }
    }
}
