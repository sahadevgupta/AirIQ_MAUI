using System;
using System.Globalization;
using AirIQ.Enums;

namespace AirIQ.Converter;

public class ProgressValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && value.ToString() != "")
        {
            if (value.ToString() == StepBarStatus.Completed.ToString())
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        return 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
