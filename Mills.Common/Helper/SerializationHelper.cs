using Mills.Common.Enum;
using Mills.Common.Model;
using System;
using System.Text;
using System.Text.Json;

namespace Mills.Common.Helper
{
    public static class SerializationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String-Representation des Objektes</returns>
        public static string Serialize(this Request request)
        {
            string result;

            switch (request.Method)
            {
                case RequestMethod.Error:
                    result = JsonSerializer.Serialize(request as ErrorRequest);
                    break;
                case RequestMethod.Login:
                    result = JsonSerializer.Serialize(request as LoginRequest);
                    break;
                case RequestMethod.LoggedIn:
                    result = JsonSerializer.Serialize(request as LoggedInRequest);
                    break;
                case RequestMethod.Logout:
                    result = JsonSerializer.Serialize(request as LogoutRequest);
                    break;
                case RequestMethod.Register:
                    result = JsonSerializer.Serialize(request as RegisterRequest);
                    break;
                case RequestMethod.Registered:
                    result = JsonSerializer.Serialize(request as RegisteredRequest);
                    break;
                case RequestMethod.SendActiveUsers:
                    result = JsonSerializer.Serialize(request as SendActiveUsersRequest);
                    break;
                case RequestMethod.Challenge:
                    result = JsonSerializer.Serialize(request as ChallengeRequest);
                    break;
                case RequestMethod.ChallengeAccepted:
                    result = JsonSerializer.Serialize(request as ChallengeAcceptedRequest);
                    break;
                case RequestMethod.ChallengeCancelled:
                    result = JsonSerializer.Serialize(request as ChallengeCancelledRequest);
                    break;
                case RequestMethod.SendChallenges:
                    result = JsonSerializer.Serialize(request as SendChallengesRequest);
                    break;
                case RequestMethod.GameStarted:
                    result = JsonSerializer.Serialize(request as GameStartedRequest);
                    break;
                case RequestMethod.SendMessage:
                    result = JsonSerializer.Serialize(request as SendMessageRequest);
                    break;
                case RequestMethod.Place:
                    result = JsonSerializer.Serialize(request as PlaceRequest);
                    break;
                case RequestMethod.Placed:
                    result = JsonSerializer.Serialize(request as PlacedRequest);
                    break;
                case RequestMethod.Move:
                    result = JsonSerializer.Serialize(request as MoveRequest);
                    break;
                case RequestMethod.Moved:
                    result = JsonSerializer.Serialize(request as MovedRequest);
                    break;
                case RequestMethod.Remove:
                    result = JsonSerializer.Serialize(request as RemoveRequest);
                    break;
                case RequestMethod.Removed:
                    result = JsonSerializer.Serialize(request as RemovedRequest);
                    break;
                case RequestMethod.Lose:
                    result = JsonSerializer.Serialize(request as LoseRequest);
                    break;
                case RequestMethod.Win:
                    result = JsonSerializer.Serialize(request as WinRequest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Keine valide Request-Methode.");
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Geparsten Request</returns>
        public static Request Deserialize(this string requestString)
        {
            Request request = JsonSerializer.Deserialize<Request>(requestString);

            switch (request.Method)
            {
                case RequestMethod.Error:
                    request = JsonSerializer.Deserialize<ErrorRequest>(requestString);
                    break;
                case RequestMethod.Login:
                    request = JsonSerializer.Deserialize<LoginRequest>(requestString);
                    break;
                case RequestMethod.LoggedIn:
                    request = JsonSerializer.Deserialize<LoggedInRequest>(requestString);
                    break;
                case RequestMethod.Logout:
                    request = JsonSerializer.Deserialize<LogoutRequest>(requestString);
                    break;
                case RequestMethod.Register:
                    request = JsonSerializer.Deserialize<RegisterRequest>(requestString);
                    break;
                case RequestMethod.Registered:
                    request = JsonSerializer.Deserialize<RegisteredRequest>(requestString);
                    break;
                case RequestMethod.SendActiveUsers:
                    request = JsonSerializer.Deserialize<SendActiveUsersRequest>(requestString);
                    break;
                case RequestMethod.Challenge:
                    request = JsonSerializer.Deserialize<ChallengeRequest>(requestString);
                    break;
                case RequestMethod.ChallengeAccepted:
                    request = JsonSerializer.Deserialize<ChallengeAcceptedRequest>(requestString);
                    break;
                case RequestMethod.ChallengeCancelled:
                    request = JsonSerializer.Deserialize<ChallengeCancelledRequest>(requestString);
                    break;
                case RequestMethod.SendChallenges:
                    request = JsonSerializer.Deserialize<SendChallengesRequest>(requestString);
                    break;
                case RequestMethod.GameStarted:
                    request = JsonSerializer.Deserialize<GameStartedRequest>(requestString);
                    break;
                case RequestMethod.SendMessage:
                    request = JsonSerializer.Deserialize<SendMessageRequest>(requestString);
                    break;
                case RequestMethod.Place:
                    request = JsonSerializer.Deserialize<PlaceRequest>(requestString);
                    break;
                case RequestMethod.Placed:
                    request = JsonSerializer.Deserialize<PlacedRequest>(requestString);
                    break;
                case RequestMethod.Move:
                    request = JsonSerializer.Deserialize<MoveRequest>(requestString);
                    break;
                case RequestMethod.Moved:
                    request = JsonSerializer.Deserialize<MovedRequest>(requestString);
                    break;
                case RequestMethod.Remove:
                    request = JsonSerializer.Deserialize<RemoveRequest>(requestString);
                    break;
                case RequestMethod.Removed:
                    request = JsonSerializer.Deserialize<RemovedRequest>(requestString);
                    break;
                case RequestMethod.Lose:
                    request = JsonSerializer.Deserialize<LoseRequest>(requestString);
                    break;
                case RequestMethod.Win:
                    request = JsonSerializer.Deserialize<WinRequest>(requestString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Keine valide Request-Methode.");
            }

            return request;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Geparsten Request</returns>
        public static Request Deserialize(this byte[] requestbytes, int byteCount)
        {
            var requestString = Encoding.UTF8.GetString(requestbytes, 0, byteCount);

            return Deserialize(requestString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Bytes der String-Representation des Objektes</returns>
        public static byte[] SerializeToBytes(this Request request)
        {
            return Encoding.UTF8.GetBytes(Serialize(request));
        }
    }
}
