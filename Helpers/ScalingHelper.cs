using System;

namespace AirIQ.Helpers;

public class ScalingHelper
{
    private static double Density
        => DeviceDisplay.MainDisplayInfo.Density;

    public static double ScaleFontSize(double size)
    {
        double multiplier = GetDensityMultiplier();
        double scaled = size * multiplier;

        return Math.Clamp(scaled, 8, 80);
    }

    public static double ScaleSpacing(double size)
    {
        return size * GetDensityMultiplier();
    }

    private static double GetDensityMultiplier()
    {
        double width = DeviceDisplay.MainDisplayInfo.Width / Density;

        return width switch
        {
            <= 360 => 0.90,                 // small phone
            <= 400 => 0.90,                 // normal phone
            <= 480 => 0.90,                 // large phone
            <= 600 => 0.90,                 // phablet
            <= 840 => 0.90,                 // small tablet / PDA
            _ => 0.90,                 // large tablet / Desktop
        };
    }
}
