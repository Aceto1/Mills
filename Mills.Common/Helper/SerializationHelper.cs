using System;
using System.Linq;
using System.Text;

namespace Mills.Common.Helper
{
    public static class SerializationHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>String-Representation des Objektes</returns>
        public static string Serialize(this object obj, string[] exclude = null)
        {
            var properties = obj.GetType().GetProperties();
            var sb = new StringBuilder();


            foreach (var property in properties)
            {
                if (exclude?.Contains(property.Name) ?? false)
                    continue;

                sb.Append($"{property.Name}={property.GetValue(obj)}\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringValue"></param>
        public static void Deserialize(this object obj, string stringValue)
        {
            var lines = stringValue.Trim('\0').Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var properties = obj.GetType().GetProperties();

            foreach (var line in lines)
            {
                var splittedLine = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (splittedLine.Length < 2)
                    continue;

                var key = splittedLine[0];
                var value = splittedLine[1];

                var prop = properties.FirstOrDefault(m => m.Name == key);
                if (prop == null)
                    continue;

                object propValue = null;

                if (prop.PropertyType.IsEnum)
                    propValue = System.Enum.Parse(prop.PropertyType, value);
                else if (prop.PropertyType.IsAssignableFrom(typeof(Int32)))
                {
                    if (Int32.TryParse(value, out var intValue))
                        propValue = intValue;
                }
                else
                    propValue = value;

                prop.SetValue(obj, propValue);
            }
        }
    }
}
