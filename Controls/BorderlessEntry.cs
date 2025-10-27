using Microsoft.Maui.Platform;

namespace AirIQ.Controls
{
    public class BorderlessEntry : Entry
    {
        public BorderlessEntry()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
                if (view is BorderlessEntry control)
                {
#if IOS
                    handler.PlatformView.BackgroundColor = Colors.Transparent.ToPlatform();
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                    handler.PlatformView.ClipsToBounds = true;
                    handler.PlatformView.Layer.BorderWidth = 0;
                    handler.PlatformView.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
#elif ANDROID
                    handler.PlatformView.InputType = Android.Text.InputTypes.TextVariationShortMessage;
                    handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                    handler.PlatformView.Background = null;

#endif
                }
            });
        }
    }
    
}
