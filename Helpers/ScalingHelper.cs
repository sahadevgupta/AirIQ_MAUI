using System;

namespace AirIQ.Helpers;

public class ScalingHelper
{
    public static double ScaleFontSize(double size)
    {
        double multiplier = GetDensityMultiplier();
        double scaled = size * multiplier;

        return Math.Max(8, Math.Min(scaled, 80));
    }

    public static double ScaleSpacing(double size)
    {
        return size * GetDensityMultiplier();
    }

    public static double GetDensityMultiplier()
    {
#if ANDROID
        return GetAndroidMultiplier();
#elif IOS
        return GetiOSMultiplier();
#else
        return 1.0;
#endif
    }

#if ANDROID
    private static double GetAndroidMultiplier()
    {
        // For Android:
        // density = scale factor (1, 1.5, 2, 3...)
        // DPI = density * 160
        double density = DeviceDisplay.MainDisplayInfo.Density;
        int densityDpi = (int)(density * 160);

        // Your original formula
        return densityDpi / 480.0;
    }
#endif

#if IOS
    private static double GetiOSMultiplier()
    {
        // iOS:
        // UIScreen.MainScreen.Scale = 2 (retina), 3 (super retina)
        double scale = DeviceDisplay.MainDisplayInfo.Density;

        // Actual PPI using native bounds
        // (Typical iPhones ~ 458ppi, iPads ~ 264ppi)
        var screen = UIKit.UIScreen.MainScreen;
        double ppi = screen.Scale * 163;  // 163 is Apple’s baseline for iPhone non-retina

        // Normalize to Android baseline (160dpi ≈ scale 1.0)
        double multiplier = ppi / 480.0;  

        // Clamp
        return Math.Max(1.0, Math.Min(multiplier, 1.5));
    }
#endif




}
