using Mills.Enum;
using Mills.Properties;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mills.Converter
{
    public class BoardPositionsToImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ObservableCollection<Tuple<ButtonPosition, int>> boardPositions || parameter is not ButtonPosition pos)
                return null;

            var collectionValue = boardPositions.FirstOrDefault(m => m.Item1 == pos)?.Item2;

            if (!collectionValue.HasValue)
                return null;

            return collectionValue.Value switch
            {
                1 => Resources.white,
                2 => Resources.black,
                _ => null,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
