using AirIQ.Controls;
using AirIQ.Services.Interfaces;
using Android.App;
using Android.Graphics.Drawables;
using Android.Views;
using Microsoft.Maui.Platform;
using Application = Microsoft.Maui.Controls.Application;
using View = Android.Views.View;
using Window =Android.Views.Window;

namespace AirIQ.Platforms.Services
{
    public class LoadingPopupService : ILoadingPopUpService
    {
        private View? _nativeView { get; set; }

        private Dialog? _dialog { get; set; }

        private bool isInitialized;

        public void ShowLoading()
        {
            InitLoaderView();
            _dialog?.Show();
        }

        public void HideLoading()
        {
            _dialog?.Hide();
            _dialog?.Dismiss();
        }

        private void InitLoaderView()
        {
            if (!isInitialized)
            {
                var loadingIndicatorView = new LoadingIndicatorView();
                var mainPage = Application.Current?.Windows[0].Page;
                if (mainPage is null)
                    return;

                var mainDisplay = DeviceDisplay.MainDisplayInfo;
                loadingIndicatorView.Layout(new Rect(0, 0, mainDisplay.Width / mainDisplay.Density, mainDisplay.Height / mainDisplay.Density));

                _nativeView = loadingIndicatorView.ToHandler(mainPage.Handler?.MauiContext!)?.PlatformView!;

                _dialog = new Dialog(Platform.CurrentActivity!);
                _dialog.RequestWindowFeature((int)WindowFeatures.NoTitle);
                _dialog.SetCancelable(false);
                _dialog.SetContentView(_nativeView);
                Window? window = _dialog.Window;
                window?.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                window?.ClearFlags(WindowManagerFlags.DimBehind);
                window?.SetBackgroundDrawable(new ColorDrawable(Colors.Transparent.ToPlatform()));

                isInitialized = true;
            }
        }
    }
}