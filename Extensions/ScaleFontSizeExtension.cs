using System;
using AirIQ.Helpers;

namespace AirIQ.Extensions;

public class ScaleFontSizeExtension : IMarkupExtension<double>
{
    public double Size { get; set; }

    public double ProvideValue(IServiceProvider serviceProvider)
        => ScalingHelper.ScaleFontSize(Size);

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        => ProvideValue(serviceProvider);
}
