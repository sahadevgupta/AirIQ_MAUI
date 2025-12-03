using System.Diagnostics;
using AirIQ.Controls;
using AirIQ.Extensions;
using AirIQ.Platforms.iOS;
using AirIQ.Services.Interfaces;
using CoreFoundation;
using CoreGraphics;
using Microsoft.Maui.Platform;
using UIKit;

namespace AirIQ.Platforms.Services
{
    public class LoadingPopupService : ILoadingPopUpService
    {
        private IDisposable _disposeAction;
        private UIView view;

        static UIImageView imageView;

        public IDisposable Show()
        {
            _disposeAction = PresentView();
            return _disposeAction;
        }

        public void Hide()
        {
            ReleaseAll(view);
        }

        private IDisposable PresentView()
        {
            DispatchQueue.MainQueue.DispatchAsync(() =>
            {
                view = null;
                view = InitDialog();
            });
            return new DisposableAction(() => DispatchQueue.MainQueue.DispatchAsync(() => {
                ReleaseAll(view);
            }));
        }

        private static UIView InitDialog()
        {
            try
            {
                var dialogView = GifLoaderImageView();

                UIApplication.SharedApplication!.KeyWindow!.AddSubview(dialogView);
                return dialogView;
            }
            catch (Exception exception)
            {
                Debug.WriteLine($"{nameof(InitDialog)} {exception.Message}");
                return new UIView();
            }
        }

        /// <summary>
        /// Create GIF loading ImageView.
        /// </summary>
        /// <returns>The loading view.</returns>
        static UIView GifLoaderImageView()
        {
            UIView loadingView = new UIView();
            loadingView.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            loadingView.BackgroundColor = ((Color)Application.Current.Resources["PopupBackground"]).ToPlatform();

            if (imageView==null)
            {
                imageView = ImageExtension.LoadGifImageWithName("loader");

                imageView.Frame = loadingView.ConvertRectFromView(loadingView.Frame, loadingView);
            }

            
            loadingView.Add(imageView);

            
            return loadingView;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1422:This call site is reachable on all platforms. 'UIApplication.KeyWindow.get' is obsoleted on: 'ios' 13.0 and later (Should not be used for applications that support multiple scenes because it returns a key window across all connected scenes.), 'maccatalyst' 13.0 and later (Should not be used for applications that support multiple scenes because it returns a key window across all connected scenes.), 'tvos' 13.0 and later (Should not be used for applications that support multiple scenes because it returns a key window across all connected scenes.).", Justification = "MAUI we uses only latest platform API.")]
        private static void ReleaseAll(UIView view)
        {
            try
            {
                bool isViewActive = UIApplication.SharedApplication.KeyWindow.Subviews.Any(t =>
                      t.RestorationIdentifier == view.RestorationIdentifier);

                if (isViewActive)
                {
                    view?.RemoveFromSuperview();
                    view?.Dispose();
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine("iOS_ProgressDialogService HandleException [{exceptionName}] \n{exceptionToString}", exception.GetType().Name, exception.ToString());

            }
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
           
            if (disposing)
            {
                _disposeAction?.Dispose();
                _disposeAction = null;
            }

            
        }

        /// <summary>
        ///     Dispose pattern
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
