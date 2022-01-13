using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ChallengeAcceptedRequest : Request
    {
        public ChallengeAcceptedRequest()
        {
            Method = RequestMethod.ChallengeAccepted;
        }

        public string SessionId { get; set; }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }
    }
}
