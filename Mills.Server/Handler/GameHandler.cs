using Mills.Common.Enum;
using Mills.Common.Helper;
using Mills.Common.Model;
using Mills.Server.Global;
using Mills.Server.Model;
using System.Linq;

namespace Mills.Server.Handler
{
    public class GameHandler
    {
        public static void StartGame(ChallengeAcceptedRequest request)
        {
            Games.Instance.AddGame(new Game()
            {
                ActiveUserId = request.ToUserId,
                UserId1 = request.ToUserId,
                UserId2 = request.FromUserId,
                Remove = false
            });
        }

        public static bool Move(MoveRequest request, out bool remove)
        {
            remove = false;

            var client = Clients.Instance.GetClient(request.SessionId);
            if (client == null)
                return false;

            var game = Games.Instance.GetGameByActivePlayerId(client.User.UserId);

            if (game == null)
                return false;

            //Leeres Feld angeklickt = Nichts passiert
            if (!game.BoardState.ContainsKey(request.From))
                return false;

            int tokenCount;

            if (game.ActiveUserId == game.UserId1)
                tokenCount = game.User1TokenCount;
            else
                tokenCount = game.User2TokenCount;

            var moves = MillsHelper.GetAvailablePositions(game.BoardState, request.From, tokenCount);

            if (moves.Contains(request.To))
            {
                // Verfügbares Feld selektiert, also erst alle als verfügbar markierten Felder löschen
                foreach (var pos in game.BoardState)
                {
                    if (pos.Value == PositionState.AvailableForMove)
                        game.BoardState.Remove(pos);
                };

                // Vorher selektierten Spielstein auf gerade ausgewählt Position verschieben und alte Position löschen
                game.BoardState.Add(request.To, game.BoardState[request.From]);
                game.BoardState.Remove(request.From);

                remove = MillsHelper.CheckForMill(game.BoardState, request.To, game.ActiveUserId == game.UserId1 ? PositionState.Player1 : PositionState.Player2);

                if (remove)
                    MillsHelper.SetRemoveTargets(game.BoardState, game.ActiveUserId == game.UserId1 ? PositionState.Player1 : PositionState.Player2);
                else
                    game.SwitchActivePlayer();

                return true;
            }

            return false;
        }

        public static bool Place(PlaceRequest request, out bool remove, out bool phaseChange)
        {
            remove = false;
            phaseChange = false;

            var client = Clients.Instance.GetClient(request.SessionId);
            if (client == null)
                return false;

            var game = Games.Instance.GetGameByActivePlayerId(client.User.UserId);

            if (game == null)
                return false;

            // Feld mit Spielstein angeklickt = Nichts passiert
            if (game.BoardState.ContainsKey(request.Position))
                return false;

            var activePlayer = game.ActiveUserId == game.UserId1 ? 1 : 2;

            game.BoardState.Add(request.Position, (PositionState)activePlayer);

            // Anzahl der platzierten Spielstein erhöhen
            game.TokensPlaced++;

            remove = MillsHelper.CheckForMill(game.BoardState, request.Position, game.ActiveUserId == game.UserId1 ? PositionState.Player1 : PositionState.Player2);

            if (activePlayer == 1)
                game.User1TokenCount++;
            else
                game.User2TokenCount++;

            // Phase 2 beginnt, wenn beide Spieler 9 Steine platziert haben
            if (game.TokensPlaced == 18)
            {
                game.Phase++;
                phaseChange = true;
            }
            else
                phaseChange = false;

            if (remove)
                MillsHelper.SetRemoveTargets(game.BoardState, game.ActiveUserId == game.UserId1 ? PositionState.Player1 : PositionState.Player2);
            else
                game.SwitchActivePlayer();

            return true;
        }

        public static bool Remove(RemoveRequest request)
        {
            var client = Clients.Instance.GetClient(request.SessionId);
            if (client == null)
                return false;

            var game = Games.Instance.GetGameByActivePlayerId(client.User.UserId);

            if (game == null)
                return false;

            //Leeres Feld angeklickt = Nichts passiert
            if (!game.BoardState.ContainsKey(request.Position))
                return false;

            // Überprüfen, ob der Wert der Posotion das Remove-Flag hat
            if (game.BoardState[request.Position].HasFlag(PositionState.RemoveTarget))
            {
                game.BoardState.Remove(request.Position);

                //Anzahl der Spielsteine für gegnerischen Spieler reduzieren
                if (game.ActiveUserId == game.UserId1)
                    game.User2TokenCount--;
                else
                    game.User1TokenCount--;

                // Flag auf restlichen Positionen entfernen
                foreach (var pos in game.BoardState)
                {
                    if (pos.Value.HasFlag(PositionState.RemoveTarget))
                        game.BoardState[pos.Key] = pos.Value & ~PositionState.RemoveTarget;
                }

                return true;
            }

            return false;
        }
    }
}
