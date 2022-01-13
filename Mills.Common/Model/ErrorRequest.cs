using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ErrorRequest : Request
    {
        public ErrorRequest()
        {
            Method = RequestMethod.Error;
        }

        public string Message { get; set; }

        public Severity Severity { get; set; }
    }
}
