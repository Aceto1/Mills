using Mills.Common.Enum;
using Mills.Common.Model.Dto;

namespace Mills.Common.Model
{
    public class SendChallengesRequest : Request
    {
        public SendChallengesRequest()
        {
            Method = RequestMethod.SendChallenges;
        }

        public ChallengeDto[] MyChallenges { get; set; }

        public ChallengeDto[] ChallengesAgainstMe { get; set; }
    }
}
