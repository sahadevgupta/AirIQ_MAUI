using System.Globalization;

using AirIQ.Enums;

using Microsoft.Maui.Controls.Shapes;

namespace AirIQ.Converter;

/// <summary>
///     Maps a <see cref="RangeBackgroundShape"/> to the <see cref="RoundRectangle"/> used as the
///     soft range-highlight Border's StrokeShape behind a calendar day cell, so a departure-to-return
///     range reads as one continuous rounded band instead of a row of disconnected squares.
/// </summary>
public class RangeShapeToCornerRadiusConverter : IValueConverter
{
    const double FullRadius = 20;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var cornerRadius = value is RangeBackgroundShape shape
            ? shape switch
            {
                RangeBackgroundShape.Left => new CornerRadius(FullRadius, 0, 0, FullRadius),
                RangeBackgroundShape.Right => new CornerRadius(0, FullRadius, FullRadius, 0),
                RangeBackgroundShape.Isolated => new CornerRadius(FullRadius),
                _ => new CornerRadius(0)
            }
            : new CornerRadius(0);

        return new RoundRectangle { CornerRadius = cornerRadius };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
