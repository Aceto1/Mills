using Mills.Common.Enum;
using Mills.Common.Model;
using Mills.Server.Model;
using System.Collections.Generic;
using System.Linq;

namespace Mills.Server.Global
{
    public class Games
    {
        private Games()
        {

        }

        private List<Game> games = new List<Game>();

        private static Games instance;

        public static Games Instance => instance ?? (instance = new Games());

        public Game GetGameBySessionId(string sessionsId)
        {
            var client = Clients.Instance.GetClient(sessionsId);
            if (client == null)
                return null;

            var game = games.FirstOrDefault(m => m.UserId1 == client.User.UserId || m.UserId2 == client.User.UserId);

            return game;
        }

        public Game GetGameByActivePlayerId(int userId)
        {
            return games.FirstOrDefault(m => m.ActiveUserId == userId);
        }

        public void AddGame(Game game)
        {
            games.Add(game);
        }

        public void RemoveGame(Game game)
        {
            games.Remove(game);
        }

        public bool IsUserIngame(int userId)
        {
            return games.Any(m => m.UserId1 == userId || m.UserId2 == userId);
        }
    }
}
