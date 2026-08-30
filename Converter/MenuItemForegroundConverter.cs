using System.Globalization;

namespace AirIQ.Converter
{
    public class MenuItemForegroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not true)
                return Colors.White;

            return Application.Current?.Resources.TryGetValue("PrimaryColor", out var color) == true
                ? color
                : Color.FromArgb("#1076BB");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
