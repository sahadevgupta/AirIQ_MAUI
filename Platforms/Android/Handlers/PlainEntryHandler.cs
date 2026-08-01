using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace AirIQ.Platforms.Handlers
{
    public class PlainEntryHandler : EntryHandler
    {
        protected override MauiAppCompatEditText CreatePlatformView()
        {
            var editText = new MauiAppCompatEditText(Context)
            {
                Background = null
            };

            editText.SetBackgroundColor(Colors.Transparent.ToPlatform());
            editText.SetPadding(0, 0, 0, 0);
            editText.SetSingleLine(true);

            return editText;
        }
    }
}
