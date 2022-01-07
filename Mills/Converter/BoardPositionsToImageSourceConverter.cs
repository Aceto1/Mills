using Mills.Common.Enum;
using Mills.Model;
using Mills.Properties;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Mills.Converter
{
    /// <summary>
    /// Converter, der für eine angegebene Position den Wert aus dem BoardState sucht und die entsprechende Resource zurückgibt.
    /// </summary>
    public class BoardPositionsToImageSourceConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ObservableDictionary<BoardPosition, PositionState> boardState || parameter is not BoardPosition position)
                return null;

            var hasValue = boardState.TryGetValue(position, out var player);

            if (!hasValue)
                return null;

            if (player.HasFlag(PositionState.Player1))
                return Resources.white;

            if (player.HasFlag(PositionState.Player2))
                return Resources.black;

            if (player.HasFlag(PositionState.AvailableForMove))
                return Resources.green;

            return null;
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
