using Mills.Common.Enum;
using Mills.Common.Model;
using Mills.Common.Model.Dto;
using Mills.Server.Global;
using Mills.Server.Model;
using System.Linq;

namespace Mills.Server.Handler
{
    public class ChallengeHandler
    {
        public static SendChallengesRequest GetChallengesForUser(int userId)
        {
            var myChallenges = Challenges.Instance.GetChallengesFromUser(userId).Select(m => new ChallengeDto()
            {
                FromUserId = m.FromUserId,
                ToUserId = m.ToUserId,
                UserName = Clients.Instance.GetClient(m.ToUserId).User.Username
            });

            var challengesAgainstMe = Challenges.Instance.GetChallengesToUser(userId).Select(m => new ChallengeDto()
            {
                FromUserId = m.FromUserId,
                ToUserId = m.ToUserId,
                UserName = Clients.Instance.GetClient(m.FromUserId).User.Username
            });

            return new SendChallengesRequest()
            {
                MyChallenges = myChallenges.ToArray(),
                ChallengesAgainstMe = challengesAgainstMe.ToArray()
            };
        }

        public static bool AddChallenge(ChallengeRequest request)
        {
            var challenge = Challenges.Instance.GetChallenge(request.FromUserId, request.ToUserId);

            if (challenge != null)
                return false;

            challenge = Challenges.Instance.GetChallenge(request.ToUserId, request.FromUserId);

            if (challenge != null)
                return false;

            challenge = new Challenge
            {
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId
            };

            Challenges.Instance.AddChallenge(challenge);

            return true;
        }

        public static bool AcceptChallenge(ChallengeAcceptedRequest request)
        {
            var challenge = Challenges.Instance.GetChallenge(request.FromUserId, request.ToUserId);

            if (challenge == null)
                return false;

            Challenges.Instance.RemoveChallenge(request.FromUserId, request.ToUserId);

            return true;
        }

        public static void CancelChallenge(ChallengeCancelledRequest request)
        {
            var challenge = Challenges.Instance.GetChallenge(request.FromUserId, request.ToUserId);

            if (challenge == null)
                return;

            Challenges.Instance.RemoveChallenge(request.FromUserId, request.ToUserId);
        }
    }
}
