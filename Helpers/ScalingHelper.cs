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

        var a = Math.Clamp(scaled, 8, 80);
        return a;
    }

    public static double ScaleSpacing(double size)
    {
        return size * GetDensityMultiplier();
    }

    private static double GetDensityMultiplier()
    {
        //double width = DeviceDisplay.MainDisplayInfo.Width / Density;

        // return width switch
        // {
        //     <= 360 => 0.90,                 // small phone
        //     <= 400 => 0.90,                 // normal phone
        //     <= 480 => 0.90,                 // large phone
        //     <= 600 => 0.90,                 // phablet
        //     <= 840 => 0.90,                 // small tablet / PDA
        //     _ => 0.90,                 // large tablet / Desktop
        // };
        switch (DeviceInfo.Current.Idiom)
        {
            case var idiom when idiom == DeviceIdiom.Phone:
                return 1.0;

            case var idiom when idiom == DeviceIdiom.Tablet:
                return 1.15;

            case var idiom when idiom == DeviceIdiom.Desktop:
                return 1.3;

            default:
                return 1.0;
        }
    }
}
