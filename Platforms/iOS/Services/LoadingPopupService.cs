using System.Diagnostics;
using AirIQ.Extensions;
using AirIQ.Platforms.iOS;
using AirIQ.Services.Interfaces;
using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace AirIQ.Platforms.Services
{
    public class LoadingPopupService : ILoadingPopUpService
    {
        private readonly object _syncRoot = new();
        private UIView? _loaderView;
        private int _activeCount;

        public IDisposable Show()
        {
            bool shouldDisplay;
            lock (_syncRoot)
            {
                shouldDisplay = ++_activeCount == 1;
            }

            if (shouldDisplay)
            {
                RunOnMainThread(DisplayLoader);
            }

            return new DisposableAction(() =>
            {
                try
                {
                    Hide();
                }
                catch (Exception)
                {
                    // ignore
                }
            });
        }

        public void Hide()
        {
            bool shouldDismiss;
            lock (_syncRoot)
            {
                if (_activeCount == 0)
                    return;

                shouldDismiss = --_activeCount == 0;
            }

            if (shouldDismiss)
            {
                RunOnMainThread(DismissLoader);
            }
        }

        private static void RunOnMainThread(Action action)
        {
            if (MainThread.IsMainThread)
                action();
            else
                MainThread.BeginInvokeOnMainThread(action);
        }

        /// <summary>Must run on the UI thread.</summary>
        private void DisplayLoader()
        {
            try
            {
                if (_loaderView is not null)
                    return; // already on screen for this show/hide session

                var window = UIApplication.SharedApplication
                    .ConnectedScenes
                    .OfType<UIWindowScene>()
                    .SelectMany(x => x.Windows)
                    .FirstOrDefault(x => x.IsKeyWindow);

                var rootVC = window?.RootViewController;
                if (rootVC?.View is null)
                    return;

                var loaderView = GifLoaderImageView();

                // IMPORTANT: Do NOT traverse to PresentedViewController
                rootVC.View.AddSubview(loaderView);
                _loaderView = loaderView;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{nameof(DisplayLoader)} {exception.Message}");
            }
        }

        /// <summary>Must run on the UI thread.</summary>
        private void DismissLoader()
        {
            try
            {
                _loaderView?.RemoveFromSuperview();
                _loaderView?.Dispose();
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{nameof(DismissLoader)} {exception.Message}");
            }
            finally
            {
                _loaderView = null;
            }
        }

        /// <summary>
        /// Create GIF loading ImageView.
        /// </summary>
        /// <returns>The loading view.</returns>
        private static UIView GifLoaderImageView()
        {
            UIView loadingView = new UIView();
            loadingView.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            loadingView.BackgroundColor = ((Color)Application.Current.Resources["PopupBackground"]).ToPlatform();

            var imageView = ImageExtension.LoadGifImageWithName("loader");
            if (imageView == null)
            {
                imageView = ImageExtension.LoadGifImageWithName("loading_anim");

                imageView.Frame = loadingView.ConvertRectFromView(loadingView.Frame, loadingView);
            }
            // Ensure the view doesn't create constraints based on its frame
            imageView.TranslatesAutoresizingMaskIntoConstraints = false;
            loadingView.Add(imageView);

            // Add horizontal and vertical center constraints
            NSLayoutConstraint.ActivateConstraints(new[] {
                imageView.CenterXAnchor.ConstraintEqualTo(loadingView.CenterXAnchor),
                imageView.CenterYAnchor.ConstraintEqualTo(loadingView.CenterYAnchor),
                // You also need to define size
                imageView.WidthAnchor.ConstraintEqualTo(200),
                imageView.HeightAnchor.ConstraintEqualTo(200)
            });

            return loadingView;
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                _activeCount = 0;
            }

            RunOnMainThread(DismissLoader);
            GC.SuppressFinalize(this);
        }
    }
}
