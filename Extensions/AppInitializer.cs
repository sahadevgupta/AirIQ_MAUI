using AirIQ.Controls;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace AirIQ.Extensions
{
    public static class AppInitializer
    {
        public static MauiAppBuilder InitializeApp(this MauiAppBuilder builder)
        {
            builder
                .ConfigureAppFonts()
                .ConfigureAppHandlers()
                .ViewInit()
                .ViewModelInit()
                .RegisterAppServices()
                .RegisterForNavigation();

            return builder;
        }


        private static MauiAppBuilder ConfigureAppFonts(this MauiAppBuilder builder)
        {
            return builder.ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
                fonts.AddFont("Roboto-ExtraBold.ttf", "RobotoExtraBold");
                fonts.AddFont("Roboto-Italic.ttf", "RobotoItalic");
                fonts.AddFont("Roboto-Medium.ttf", "RobotoMedium");
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("Roboto-SemiBold.ttf", "RobotoSemiBold");
                fonts.AddFont("fa-solid-900.ttf", "MyFont");
            });
        }

        private static MauiAppBuilder ConfigureAppHandlers(this MauiAppBuilder builder)
        {
            EditorHandler.Mapper.AppendToMapping("NestedScroll", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
                handler.PlatformView.Background = null;
                handler.PlatformView.SetOnTouchListener(new EditorTouchListener());
#endif
            });

            EditorHandler.Mapper.AppendToMapping("BorderlessEditor", (handler, view) =>
            {
                if (view is not BorderlessEditor)
                    return;
#if IOS
                handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
                handler.PlatformView.TextContainerInset = UIKit.UIEdgeInsets.Zero;
                handler.PlatformView.TextContainer.LineFragmentPadding = 0;
#elif ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
                handler.PlatformView.Background = null;
#endif
            });

            EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
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

            SearchBarHandler.Mapper.AppendToMapping(nameof(SearchView), (handler, view) =>
           {
#if IOS
                handler.PlatformView.BackgroundColor = Colors.Transparent.ToPlatform();
                //handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                handler.PlatformView.ClipsToBounds = true;
                handler.PlatformView.Layer.BorderWidth = 0;
                handler.PlatformView.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
#elif ANDROID

               handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
               handler.PlatformView.Background = null;

#endif
           });

            return builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CustomDropdown, AirIQ.Platforms.Handlers.CustomDropdownHandler>();
                handlers.AddHandler<Entry, AirIQ.Platforms.Handlers.PlainEntryHandler>();
                handlers.AddHandler<BorderlessEntry, AirIQ.Platforms.Handlers.PlainEntryHandler>();
            });


        }
    }

#if ANDROID



    public class EditorTouchListener : Java.Lang.Object, Android.Views.View.IOnTouchListener
    {
        public bool OnTouch(Android.Views.View? v, Android.Views.MotionEvent? e)
        {
            switch (e?.Action)
            {
                case Android.Views.MotionEventActions.Down:
                case Android.Views.MotionEventActions.Move:
                    v?.Parent?.RequestDisallowInterceptTouchEvent(true);
                    break;

                case Android.Views.MotionEventActions.Up:
                case Android.Views.MotionEventActions.Cancel:
                    v?.Parent?.RequestDisallowInterceptTouchEvent(false);
                    break;
            }

            return false;
        }
    }
#endif
}