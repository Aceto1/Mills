using Mills.Common.Enum;

namespace Mills.Common.Model
{
    public class WinRequest : Request
    {
        public WinRequest()
        {
            Method = RequestMethod.Win;
        }
    }
}
