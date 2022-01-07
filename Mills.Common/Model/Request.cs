using Mills.Common.Enum;
using Mills.Common.Helper;
using System.Linq;

namespace Mills.Common.Model
{
    public class Request
    {
        public Request(string requestString)
        {
            var requestLines = requestString.Split("\n");

            Method = requestLines[0].ToRequestMethod();

            RequestBody = RequestBodyFactory.NewRequestBody(Method);
            RequestBody.Deserialize(requestLines[1..].Aggregate((prev, curr) => prev + "\n" + curr));
        }

        public RequestMethod Method { get; set; }

        public RequestBody RequestBody { get; set; }
    }
}
