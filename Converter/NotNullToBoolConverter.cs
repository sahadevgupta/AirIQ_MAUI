using System.Globalization;

namespace AirIQ.Converter;

/// <summary>Converts any reference/nullable value to True when it is non-null - typically used to show/hide a "clear" affordance.</summary>
public class NotNullToBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
