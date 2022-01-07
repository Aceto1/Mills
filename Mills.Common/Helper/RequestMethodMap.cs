using Mills.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mills.Common.Helper
{
    internal static class RequestMethodMap
    {
        private static readonly Dictionary<string, RequestMethod> map = new Dictionary<string, RequestMethod>()
        {
            { "[LOGIN]", RequestMethod.Login },
            { "[LOGOUT]", RequestMethod.Logout },
            { "[REGISTER]", RequestMethod.Register },
            { "[SENDMESSAGE]", RequestMethod.SendMessage },
        };

        public static RequestMethod ToRequestMethod(this string stringMethod)
        {
            if (map.TryGetValue(stringMethod, out RequestMethod method))
                return method;

            throw new ArgumentException($"The given string {stringMethod} is not a valid request method.");
        }

        public static string ToString(this RequestMethod method)
        {
            return map.First(m => m.Value == method).Key;
        }
    }
}
