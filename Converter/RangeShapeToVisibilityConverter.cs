using System.Globalization;

using AirIQ.Enums;

namespace AirIQ.Converter;

/// <summary>Shows the soft range-highlight layer for every shape except <see cref="RangeBackgroundShape.None"/>.</summary>
public class RangeShapeToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is RangeBackgroundShape shape && shape != RangeBackgroundShape.None;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
