using System.Linq;
using System.Text;

namespace Mills.Common.Model
{
    public abstract class RequestBody
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String-Representation des Objektes</returns>
        public string Serialize()
        {
            var properties = this.GetType().GetProperties();
            var sb = new StringBuilder();


            foreach (var property in properties)
            {
                sb.AppendLine($"{property.Name}={property.GetValue(this)}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue"></param>
        public void Deserialize(string stringValue)
        {
            var lines = stringValue.Split('\n');
            var properties = this.GetType().GetProperties();

            foreach (var line in lines)
            {
                var splittedLine = line.Split("=");

                var key = splittedLine[0];
                var value = splittedLine[1];

                var prop = properties.FirstOrDefault(m => m.Name == key);
                if (prop != null)
                {
                    prop.SetValue(this, value);
                }
            }
        }

        public override string ToString()
        {
            return Serialize();
        }
    }
}
