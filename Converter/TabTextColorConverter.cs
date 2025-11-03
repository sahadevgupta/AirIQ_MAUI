using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirIQ.Converter
{
    public class TabTextColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is Enums.BookingTypes selected && parameter is Enums.BookingTypes current)
            {
                return selected == current ? Colors.White : Colors.Black;
            }
            return Colors.Black;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
