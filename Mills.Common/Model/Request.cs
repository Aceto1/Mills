using Mills.Common.Enum;
using Mills.Common.Helper;
using System;
using System.Linq;
using System.Text;

namespace Mills.Common.Model
{
    public abstract class Request
    {
        public Request()
        {

        }

        public void Parse(string requestString)
        {
            var requestLines = requestString.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            this.Deserialize(requestLines.Aggregate((prev, curr) => prev + "\n" + curr));
        }

        public abstract RequestMethod Method { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(RequestMethodMap.ToString(Method) + "\n");

            sb.Append(this.Serialize(new string[1] { nameof(Method) }));

            return sb.ToString();
        }

        public byte[] GetBytes()
        {
            return Encoding.UTF8.GetBytes(this.ToString());
        }
    }
}
