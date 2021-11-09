using Mills.Enum;
using Mills.Model;
using Mills.Properties;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mills.Converter
{
    public class BoardPositionsToImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ObservableDictionary<BoardPosition, PositionState> boardPositions || parameter is not BoardPosition pos)
                return null;

            var hasValue = boardPositions.TryGetValue(pos, out var player);

            if (!hasValue)
                return null;

            return player switch
            {
                PositionState.Player1 => Resources.white,
                PositionState.Player2 => Resources.black,
                PositionState.AvailableForMove => Resources.green,
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
