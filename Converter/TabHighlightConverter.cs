using AirIQ.Enums;
using System.Globalization;

namespace AirIQ.Converter
{
    class TabHighlightConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is BookingTypes selected && parameter is BookingTypes current)
                return selected == current ? Color.FromArgb("#3B82F6") : Color.FromArgb("#E5E7EB");
            return Color.FromArgb("#E5E7EB");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
