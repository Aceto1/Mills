using Mills.Common.Enum;
using Mills.Common.Model;
using System;

namespace Mills.Common.Helper
{
    public static class RequestBodyFactory
    {
        public static RequestBody NewRequestBody(RequestMethod method)
        {
            RequestBody result = null;

            switch (method)
            {
                case RequestMethod.Login:
                    result = new LoginBody();
                    break;
                case RequestMethod.Logout:
                    break;
                case RequestMethod.Register:
                    break;
                case RequestMethod.Move:
                    break;
                case RequestMethod.GetActiveUsers:
                    break;
                case RequestMethod.SendMessage:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}
