using AirIQ.Controls;

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
                .RefitClientInit()
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

            return builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<CustomDropdown, AirIQ.Platforms.Handlers.CustomDropdownHandler>();
#if ANDROID
                handlers.AddHandler(typeof(Shell), typeof(AirIQ.Platforms.Handlers.CustomShellRenderer));
#endif
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