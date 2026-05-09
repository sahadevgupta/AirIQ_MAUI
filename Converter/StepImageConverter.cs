using System;
using System.Globalization;
using AirIQ.Enums;
using AirIQ.Extensions;

namespace AirIQ.Converter;

public class StepImageConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && value.ToString() != "")
        {
            var fontImage = new FontImageSource
            {
                Glyph = FontAwesomeIcons.Check,               // your icon unicode
                FontFamily = "MyFont",   // or FontAwesome, etc.
                Color = Colors.White,
                Size = 24
            };
            return value.ToString() == StepBarStatus.Completed.ToString() ? fontImage : (object)"";
        }
        return "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}