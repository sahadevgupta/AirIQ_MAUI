using System;
using System.Diagnostics;
using System.Drawing;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ImageIO;
using UIKit;

namespace AirIQ.Platforms.iOS;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1416:This call site is reachable on: 'ios', 'maccatalyst', 'tvos'.", Justification = "MAUI we uses only latest platform API.")]
public static class ImageExtension
{
    static List<CGImage> cgImagesList;
    static List<NSObject> frameImageslist;
    static double realDuration = 0.1;
    static float imageSourceLoopCount;
    static List<NSNumber> _framePercentageDurations;
    static double _totalFrameDuration;

    public static UIImageView LoadGifImageWithName(string imageName)
    {
        NSUrl url = NSBundle.MainBundle.GetUrlForResource(imageName, ".gif");

        var sourceRef = CGImageSource.FromUrl(NSUrl.FromString(url.AbsoluteString));

        UIImageView imageView = new UIImageView();

        if (frameImageslist != null)
        {
            if (imageView!.Layer!.AnimationKeys != null)
            {
                imageView.Layer.RemoveAllAnimations();
            }
        }
        else
        {
            CreateAnimatedImageView(sourceRef);
        }

        SetImageProperties(imageView, sourceRef);

        imageView.ContentMode = UIViewContentMode.Center;

        return imageView;
    }

    private static void SetImageProperties(UIImageView imageView, CGImageSource sourceRef)
    {
        try
        {
            var imageSourceProperties = sourceRef.GetProperties(null);
            var imageSourceGIFProperties = imageSourceProperties.Dictionary["{GIF}"];
            var loopCount = imageSourceGIFProperties.ValueForKey(new NSString("LoopCount"));
            imageSourceLoopCount = float.Parse(loopCount.ToString());

            var firstFrame = cgImagesList[0];
            imageView.Frame = new RectangleF(0.0f, 0.0f, firstFrame.Width, firstFrame.Height);
            imageView.Layer.AddAnimation(GetAnimation(), "contents");
            imageSourceGIFProperties.Dispose();
        }
        catch (Exception exception)
        {
            Debug.WriteLine("KeyboardBehavior HandleException [{exceptionName}] \n{exceptionToString}", exception.GetType().Name, exception.ToString());
        }
    }

    private static CAKeyFrameAnimation GetAnimation()
    {
        var frameAnimation = new CAKeyFrameAnimation();
        frameAnimation.KeyPath = "contents";

        if (imageSourceLoopCount <= 0.0f)
        {
            frameAnimation.RepeatCount = float.MaxValue;
        }
        else
        {
            frameAnimation.RepeatCount = imageSourceLoopCount;
        }

        frameAnimation.CalculationMode = CAAnimation.AnimationDiscrete;
        frameAnimation.Values = frameImageslist.ToArray();
        frameAnimation.Duration = _totalFrameDuration;
        frameAnimation.KeyTimes = _framePercentageDurations.ToArray();
        frameAnimation.RemovedOnCompletion = false;

        return frameAnimation;
    }

    private static void CreateAnimatedImageView(CGImageSource imageSource)
    {
        var frameCount = (int)imageSource.ImageCount;

        var frameImages = new List<NSObject>(frameCount);
        var frameCGImages = new List<CGImage>(frameCount);
        var frameDurations = new List<double>(frameCount);
        var totalFrameDuration = 0.0;

        for (int i = 0; i < frameCount; i++)
        {
            var frameImage = imageSource.CreateImage(i, null);
            frameCGImages.Add(frameImage);
            frameImages.Add(NSObject.FromObject(frameImage));

            var properties = imageSource.GetProperties(i, null);
            var duration = properties.Dictionary["{GIF}"];
            duration.Dispose();

            frameDurations.Add(realDuration);
            totalFrameDuration += realDuration;
            frameImage.Dispose();
        }
        cgImagesList = frameCGImages;
        frameImageslist = frameImages;
        _totalFrameDuration = totalFrameDuration;
        var framePercentageDurations = new List<NSNumber>(frameCount);
        var framePercentageDurationsDouble = new List<double>(frameCount);
        NSNumber currentDurationPercentage = 0.0f;
        double currentDurationDouble = 0.0f;
        for (int i = 0; i < frameCount; i++)
        {
            if (i != 0)
            {
                var previousDuration = frameDurations[i - 1];
                var previousDurationPercentage = framePercentageDurationsDouble[i - 1];
                var number = previousDurationPercentage + (previousDuration / totalFrameDuration);
                currentDurationDouble = number;
                currentDurationPercentage = new NSNumber(number);
            }
            framePercentageDurationsDouble.Add(currentDurationDouble);
            framePercentageDurations.Add(currentDurationPercentage);
        }

        _framePercentageDurations = framePercentageDurations;

    }

}