using AirIQ.Controls;
using AirIQ.Extensions;
using AirIQ.Services.Interfaces;
using Mopups.Interfaces;

namespace AirIQ.Services;

/// <summary>
/// Shared, cross-platform implementation. This intentionally does NOT manipulate native views
/// directly (that approach - manually building a page, extracting its raw native PlatformView,
/// and splicing it into the native root view - broke animated content: SkiaSharp's animation
/// ticker only runs once the element is attached to a real MAUI Window, which a raw spliced-in
/// view never is). Presenting the loader as a Mopups popup keeps it a properly window-attached
/// page. See LoadingIndicatorView.xaml for the full incident history.
/// </summary>

/*
public class LoadingPopupService(IPopupNavigation popupNavigation) : ILoadingPopUpService
{
    private readonly object _syncRoot = new();
    private int _activeCount;
    private LoadingIndicatorView? _popup;

    // Tracks the in-flight Show()/PushAsync call so a fast Hide() can't race ahead of it - see
    // DismissLoaderAsync for why that race is the whole reason this field exists.
    private Task _showTask = Task.CompletedTask;

    public IDisposable Show()
    {
        bool shouldDisplay;
        lock (_syncRoot)
        {
            shouldDisplay = ++_activeCount == 1;
        }

        if (shouldDisplay)
        {
            _showTask = DisplayLoaderAsync();
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
        _ = HideAsync();
    }

    public Task HideAsync()
    {
        bool shouldDismiss;
        lock (_syncRoot)
        {
            if (_activeCount == 0)
                return Task.CompletedTask;

            shouldDismiss = --_activeCount == 0;
        }

        return shouldDismiss ? DismissLoaderAsync() : Task.CompletedTask;
    }

    private async Task DisplayLoaderAsync()
    {
        try
        {
            var popup = new LoadingIndicatorView();
            _popup = popup;
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await popupNavigation.PushAsync(popup, animate: false);
            });
        }
        catch (Exception)
        {
            // best-effort UI, never let a loader failure crash the caller
        }
    }

    private async Task DismissLoaderAsync()
    {
        try
        {
            // IMPORTANT: wait for Show()'s PushAsync to actually land before trying to remove
            // anything. Without this, a fast operation can call Hide() (e.g. via the `using`
            // block around LoadingService.Show() completing quickly) before the popup has been
            // pushed - PopupStack.Contains(popup) then finds nothing, this dismiss silently
            // no-ops, and when the push finally lands moments later the popup is left stuck on
            // screen with nothing left to ever remove it.
            await _showTask.ConfigureAwait(false);

            var popup = _popup;
            _popup = null;
            if (popup is null)
                return;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (popupNavigation.PopupStack.Contains(popup))
                {
                    await popupNavigation.RemovePageAsync(popup, animate: false);
                }
            });
        }
        catch (Exception)
        {
            // ignore
        }
    }

    public void Dispose()
    {
        lock (_syncRoot)
        {
            _activeCount = 0;
        }

        _ = DismissLoaderAsync();
    }
   
}
 */
