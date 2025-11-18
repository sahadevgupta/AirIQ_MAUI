using AirIQ.Controls;
using AirIQ.Services.Interfaces;
using Microsoft.Maui.Platform;
using UIKit;

namespace AirIQ.Platforms.Services
{
    public class LoadingPopupService : ILoadingPopUpService
    {
        private static UIView? loadingView;
        private bool isInitialized;

        private void InitLoader()
        {
            if (!isInitialized)
            {
                var loadingIndicatorView = new LoadingIndicatorView();
                var mainPage = Application.Current?.Windows[0].Page;
                if (mainPage is null)
                    return;

                loadingView = new UIView();

                loadingView.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
                loadingView.BackgroundColor = Color.FromArgb("#A0000000").ToPlatform();
                var mainDisplay = DeviceDisplay.MainDisplayInfo;
                loadingIndicatorView.Layout(new Rect(0, 0, mainDisplay.Width / mainDisplay.Density, mainDisplay.Height / mainDisplay.Density));

                var view = loadingIndicatorView.ToHandler(mainPage.Handler?.MauiContext!).PlatformView;
                view!.Frame = loadingView.ConvertRectFromView(loadingView.Frame, loadingView);
                loadingView.Add(view);

                isInitialized = true;
            }
        }

        public void HideLoading()
        {
            loadingView?.RemoveFromSuperview();
        }

        public void ShowLoading()
        {
            InitLoader();
            UIApplication.SharedApplication.KeyWindow.AddSubview(loadingView!);
        }

    }
}
