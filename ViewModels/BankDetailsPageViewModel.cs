using AirIQ.Constants;
using AirIQ.Resources.Strings;
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
                var displayName = appKey == "GooglePay" ? AppResource.GooglePay : appKey;

                switch (result.Status)
                {
                    case UpiAppLaunchStatus.Success:
                        return;

                    case UpiAppLaunchStatus.NotInstalled:
                        await DialogService.DisplayAlertAsync(AppResource.AppNotFoundTitle, string.Format(AppResource.AppNotInstalledOnDeviceFormat, displayName), AppResource.OK);
                        return;

                    case UpiAppLaunchStatus.InvalidApp:
                        await DialogService.DisplayAlertAsync(AppResource.UnsupportedAppTitle, AppResource.UpiAppNotSupportedYet, AppResource.OK);
                        return;

                    case UpiAppLaunchStatus.LaunchFailed:
                        await DialogService.DisplayAlertAsync(AppResource.UnableToOpenAppTitle, result.ErrorMessage ?? AppResource.PleaseTryAgain, AppResource.OK);
                        return;

                    default:
                        await DialogService.DisplayAlertAsync(AppResource.UnableToOpenAppTitle, AppResource.PleaseTryAgain, AppResource.OK);
                        return;
                }
            }
            catch (Exception)
            {
                await DialogService.DisplayAlertAsync(AppResource.UnableToOpenAppTitle, AppResource.PleaseTryAgain, AppResource.OK);
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
                    await DialogService.DisplayAlertAsync(AppResource.PaymentSuccessfulTitle, AppResource.PaymentCompletedSuccessfully, AppResource.OK);
                    break;

                case UpiPaymentStatus.Failure:
                    await DialogService.DisplayAlertAsync(AppResource.PaymentFailedTitle, AppResource.PaymentNotCompletedTryAgain, AppResource.OK);
                    break;

                case UpiPaymentStatus.Submitted:
                    await DialogService.DisplayAlertAsync(AppResource.PaymentPendingTitle, AppResource.PaymentSubmittedPendingConfirmation, AppResource.OK);
                    break;

                case UpiPaymentStatus.Cancelled:
                    await DialogService.DisplayAlertAsync(AppResource.PaymentCancelledTitle, AppResource.PaymentWasCancelled, AppResource.OK);
                    break;

                default:
                    await DialogService.DisplayAlertAsync(AppResource.PaymentStatusTitle, AppResource.ReturnedFromUpiApp, AppResource.OK);
                    break;
            }
        }
    }
}