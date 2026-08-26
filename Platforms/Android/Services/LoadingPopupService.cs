using AirIQ.Controls;
using AirIQ.Extensions;
using AirIQ.Services.Interfaces;
using Android.Views;
using Microsoft.Maui.Platform;
using Application = Microsoft.Maui.Controls.Application;
using View = Android.Views.View;

namespace AirIQ.Platforms.Services
{
    public class LoadingPopupService : ILoadingPopUpService
    {
        private readonly object _syncRoot = new();
        private View? _nativeView;
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

        /// <summary>
        /// Must run on the UI thread. Creates a fresh native overlay for this show/hide
        /// session so state left over from navigation (page/activity changes) can't cause
        /// the loader to silently fail to reattach.
        /// </summary>
        private void DisplayLoader()
        {
            try
            {
                var windows = Application.Current?.Windows;
                var mainPage = windows is { Count: > 0 } ? windows[0].Page : null;
                var mauiContext = mainPage?.Handler?.MauiContext;
                if (mauiContext is null)
                    return;

                var rootView = Platform.CurrentActivity?.Window?.DecorView as ViewGroup;
                if (rootView is null)
                    return;

                var loadingIndicatorView = new LoadingIndicatorView();
                var mainDisplay = DeviceDisplay.MainDisplayInfo;
                loadingIndicatorView.Layout(new Rect(0, 0, mainDisplay.Width / mainDisplay.Density, mainDisplay.Height / mainDisplay.Density));

                var nativeView = loadingIndicatorView.ToHandler(mauiContext)?.PlatformView as View;
                if (nativeView is null)
                    return;

                rootView.AddView(nativeView);
                _nativeView = nativeView;
            }
            catch (Exception)
            {
                // best-effort UI, never let a loader failure crash the caller
            }
        }

        /// <summary>Must run on the UI thread.</summary>
        private void DismissLoader()
        {
            try
            {
                if (_nativeView?.Parent is ViewGroup parent)
                {
                    parent.RemoveView(_nativeView);
                }
            }
            catch (Exception)
            {
                // ignore
            }
            finally
            {
                _nativeView = null;
            }
        }

        public void Dispose()
        {
            lock (_syncRoot)
            {
                _activeCount = 0;
            }

            RunOnMainThread(DismissLoader);
        }
    }
}