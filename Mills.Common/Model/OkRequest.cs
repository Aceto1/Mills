using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class OkRequest : Request
    {
        public override RequestMethod Method => RequestMethod.Ok;

        public string SessionId { get; set; }
    }
}
