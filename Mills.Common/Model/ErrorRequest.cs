using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class ErrorRequest : Request
    {
        public override RequestMethod Method => RequestMethod.Error;

        public string Message { get; set; }

        public Severity Severity { get; set; }
    }
}
