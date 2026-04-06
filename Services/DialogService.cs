using System;
using AirIQ.Extensions;
using AirIQ.Popups;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Interfaces;

namespace AirIQ.Services;

public class DialogService(IPopupNavigation popupNavigation) : IDialogService
{
    public async Task ShowStatusAlertAsync(string message, bool response = true, int timeout = 2500)
    {
        try
        {
            if (popupNavigation == null)
            {
                System.Diagnostics.Debug.WriteLine("DialogService: popupNavigation is null");
                return;
            }

            var appResources = Application.Current?.Resources;
            if (appResources == null)
            {
                System.Diagnostics.Debug.WriteLine("DialogService: Application resources are null");
                return;
            }

            var greenColor = appResources.TryGetValue("Green", out var green) && green is Color g ? g : Colors.Green;
            var redColor = appResources.TryGetValue("Red", out var red) && red is Color r ? r : Colors.Red;

            var popup = new CustomDialogPopup
            {
                Message = message,
                Icon = response ? FontAwesomeIcons.CheckCircle : FontAwesomeIcons.TimesCircle,
                IconTintColor = response ? greenColor : redColor
            };

            bool isMyPopupOpen = popupNavigation.PopupStack?.Any(p => p is CustomDialogPopup) ?? false;
            if (!isMyPopupOpen)
            {
                await popupNavigation.PushAsync(popup);
                System.Threading.Timer? timer = null;
                timer = new System.Threading.Timer(async (obj) =>
                {
                    try
                    {
                        if (popupNavigation?.PopupStack?.Count > 0)
                        {
                            await popupNavigation.PopAsync(animate: false);
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // Popup already disposed, this is expected
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error removing dialog popup: {ex.Message}");
                    }
                    finally
                    {
                        timer?.Dispose();
                    }
                }, null, timeout, System.Threading.Timeout.Infinite);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in ShowAlertDialog: {ex.Message}");
        }
    }

    public async void ShowToast(string message, double toastfontSize = 14, ToastDuration toastDuration = ToastDuration.Short)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ToastDuration duration = toastDuration;
            double fontSize = Helpers.ScalingHelper.ScaleFontSize(toastfontSize);

            var toast = Toast.Make(message, duration, fontSize);

            await toast.Show(cancellationTokenSource.Token);
        });
    }
}

