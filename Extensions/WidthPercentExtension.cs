using System;

namespace AirIQ.Extensions;

/// <summary>
/// Helps to get a value for WidthRequest/HeightRequest that is relative to
/// the width of
/// the current main display on the device.
/// </summary>
[ContentProperty(nameof(WidthPercentExtension.MarginPercentage))]
public class WidthPercentExtension : IMarkupExtension
{
    /// <summary>
    /// Percentage to use; typically, a value between 0 and 100.
    /// </summary>
    public double MarginPercentage { get; set; }
    public WidthPercentExtension(double marginPercentage)
    {
        MarginPercentage = marginPercentage;
    }
    public WidthPercentExtension() : this(1d)
    {
    }
    public object ProvideValue(IServiceProvider serviceProvider)
    {
        var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        return (screenWidth - (screenWidth * MarginPercentage));
        //return (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) * MarginPercentage;
    }
}
