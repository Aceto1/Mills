using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ChallengeRequest : Request
    {
        public ChallengeRequest()
        {
            Method = RequestMethod.Challenge;
        }

        public string SessionId { get; set; }

        public int FromUserId { get; set; }

        public int ToUserId { get; set; }
    }
}
