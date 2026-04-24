using System;
using System.Globalization;
using AirIQ.Enums;

namespace AirIQ.Converter;

public class StepImageConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && value.ToString() != "")
        {
            return value.ToString() == StepBarStatus.Completed.ToString() ? ImageSource.FromFile("circle_plus.png") : (object)"";
        }
        return "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}