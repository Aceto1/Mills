using Mills.Common.Enum;
using Mills.Common.Model;
using System;
using System.Linq;

namespace Mills.Common.Helper
{
    public class RequestHelper
    {
        public static Request ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            RequestMethod method;

            try
            {
                method = RequestMethodMap.ToRequestMethod(requestLines[0]);
            }
            catch (Exception)
            {
                return null;
            }

            Request request = null;

            switch (method)
            {
                case RequestMethod.Ok:
                    request = new OkRequest();
                    break;
                case RequestMethod.Error:
                    request = new ErrorRequest();
                    break;
                case RequestMethod.Login:
                    request = new LoginRequest();
                    break;
                case RequestMethod.Logout:
                    request = new LogoutRequest();
                    break;
                case RequestMethod.Register:
                    request = new RegisterRequest();
                    break;
                case RequestMethod.GetActiveUsers:
                    break;
                case RequestMethod.Challenge:
                    break;
                case RequestMethod.SendMessage:
                    break;
                case RequestMethod.ReceiveMessage:
                    break;
                case RequestMethod.Place:
                    break;
                case RequestMethod.Move:
                    break;
                case RequestMethod.Remove:
                    break;
                case RequestMethod.Lose:
                    break;
                case RequestMethod.Win:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Keine valide Request-Methode.");
            }

            var requestBodyString = requestLines[1..].Aggregate((prev, curr) => prev + "\n" + curr);

            request.Deserialize(requestBodyString);

            return request;
        }
    }
}
