using System.Globalization;

namespace AirIQ.Converter
{
    public class MenuItemBackgroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not true)
                return Colors.Transparent;

            return Application.Current?.Resources.TryGetValue("MenuItemSelectedBackground", out var color) == true
                ? color
                : Color.FromArgb("#D6EAF8");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
