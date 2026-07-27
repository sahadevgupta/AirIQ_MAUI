using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class BankDetailsPageViewModel : BaseViewModel
    {
        private readonly IUpiAppLaunchService _upiAppLaunchService;
        private readonly IUpiPaymentCallbackService _upiPaymentCallbackService;

        [ObservableProperty]
        private string _qrValue = UpiPaymentConstants.BuildPaymentQueryString();

        public BankDetailsPageViewModel(IViewModelParameters viewModelParameters,
            IUpiAppLaunchService upiAppLaunchService,
            IUpiPaymentCallbackService upiPaymentCallbackService) : base(viewModelParameters)
        {
            _upiAppLaunchService = upiAppLaunchService;
            _upiPaymentCallbackService = upiPaymentCallbackService;
            _upiPaymentCallbackService.CallbackReceived += OnUpiCallbackReceived;
        }

        [RelayCommand]
        private async Task OpenUpiAppAsync(string appKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(appKey))
                    return;

                var result = await _upiAppLaunchService.LaunchAsync(appKey);
                var displayName = appKey == "GooglePay" ? "Google Pay" : appKey;

                switch (result.Status)
                {
                    case UpiAppLaunchStatus.Success:
                        return;

                    case UpiAppLaunchStatus.NotInstalled:
                        await DialogService.DisplayAlertAsync("App not found", $"{displayName} is not installed on this device.", "OK");
                        return;

                    case UpiAppLaunchStatus.InvalidApp:
                        await DialogService.DisplayAlertAsync("Unsupported app", "This UPI app is not supported yet.", "OK");
                        return;

                    case UpiAppLaunchStatus.LaunchFailed:
                        await DialogService.DisplayAlertAsync("Unable to open app", result.ErrorMessage ?? "Please try again.", "OK");
                        return;

                    default:
                        await DialogService.DisplayAlertAsync("Unable to open app", "Please try again.", "OK");
                        return;
                }
            }
            catch (Exception)
            {
                await DialogService.DisplayAlertAsync("Unable to open app", "Please try again.", "OK");
            }
        }

        public override Task LoadDataWhenOnAppearing()
        {
            if (_upiPaymentCallbackService.TryConsumePending(out var callbackData) && callbackData != null)
            {
                _ = HandleCallbackUiAsync(callbackData);
            }

            return base.LoadDataWhenOnAppearing();
        }

        public override void Destory()
        {
            _upiPaymentCallbackService.CallbackReceived -= OnUpiCallbackReceived;
            base.Destory();
        }

        private async void OnUpiCallbackReceived(object? sender, UpiPaymentCallbackData callbackData)
        {
            await HandleCallbackUiAsync(callbackData);
        }

        private async Task HandleCallbackUiAsync(UpiPaymentCallbackData callbackData)
        {
            switch (callbackData.Status)
            {
                case UpiPaymentStatus.Success:
                    await DialogService.DisplayAlertAsync("Payment successful", "Payment completed successfully.", "OK");
                    break;

                case UpiPaymentStatus.Failure:
                    await DialogService.DisplayAlertAsync("Payment failed", "Payment was not completed. Please try again.", "OK");
                    break;

                case UpiPaymentStatus.Submitted:
                    await DialogService.DisplayAlertAsync("Payment pending", "Payment is submitted and pending confirmation.", "OK");
                    break;

                case UpiPaymentStatus.Cancelled:
                    await DialogService.DisplayAlertAsync("Payment cancelled", "Payment was cancelled.", "OK");
                    break;

                default:
                    await DialogService.DisplayAlertAsync("Payment status", "Returned from UPI app.", "OK");
                    break;
            }
        }
    }
}