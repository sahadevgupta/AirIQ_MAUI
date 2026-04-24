using System;
using System.Globalization;
using AirIQ.Enums;

namespace AirIQ.Converter;

public class StepColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && value.ToString() != "")
        {
            if (value.ToString() == StepBarStatus.Completed.ToString())
            {
                return Color.FromArgb("#127ABF");
            }
            else if (value.ToString() == StepBarStatus.InProgress.ToString())
            {
                return Color.FromArgb("#127ABF");
            }
            else
            {
                return Colors.Silver;
            }
        }
        return "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}