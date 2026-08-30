using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace AirIQ.Platforms.Handlers
{
    public class PlainEntryHandler : EntryHandler
    {
        protected override MauiTextField CreatePlatformView()
        {
            var textField = new MauiTextField
            {
                BorderStyle = UITextBorderStyle.None,
                BackgroundColor = UIColor.Clear,
                ClipsToBounds = true,
            };

            textField.Layer.BorderWidth = 0;
            textField.Layer.BorderColor = UIColor.Clear.CGColor;

            return textField;
        }
    }
}
