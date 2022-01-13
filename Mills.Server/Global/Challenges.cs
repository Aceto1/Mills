using Mills.Server.Model;
using System.Collections.Generic;
using System.Linq;

namespace Mills.Server.Global
{
    public class Challenges
    {
        private Challenges()
        {

        }

        private static Challenges instance;

        public static Challenges Instance => instance ?? (instance = new Challenges());

        private List<Challenge> challenges = new List<Challenge>();

        public void AddChallenge(Challenge challenge)
        {
            challenges.Add(challenge);
        }

        public List<Challenge> GetChallengesFromUser(int fromUserId)
        {
            return challenges.Where(m => m.FromUserId == fromUserId).ToList();
        }

        public List<Challenge> GetChallengesToUser(int toUserId)
        {
            return challenges.Where(m => m.ToUserId == toUserId).ToList();
        }

        public Challenge GetChallenge(int fromUserId, int toUserId)
        {
            return challenges.FirstOrDefault(m => m.FromUserId == fromUserId && m.ToUserId == toUserId);
        }

        public void RemoveChallenge(int fromUserId, int toUserId)
        {
            var challenge = challenges.FirstOrDefault(m => m.FromUserId == fromUserId && m.ToUserId == toUserId);

            if(challenge != null)
                challenges.Remove(challenge);
        }
    }
}
