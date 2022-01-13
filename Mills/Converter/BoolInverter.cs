using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mills.Converter
{
    public class BoolInverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool boolValue)
                return false;

            return !boolValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not bool boolValue)
                return false;

            return !boolValue;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
