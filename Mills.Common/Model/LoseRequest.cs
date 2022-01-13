using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class LoseRequest : Request
    {
        public LoseRequest()
        {
            Method = RequestMethod.Lose;
        }
    }
}
