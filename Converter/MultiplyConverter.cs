using System;
using System.Globalization;

namespace AirIQ.Converter;

public class MultiplyConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        double v = System.Convert.ToDouble(value);
        double multiplier = System.Convert.ToDouble(parameter);
        return v * multiplier;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
