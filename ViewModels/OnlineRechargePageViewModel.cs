using System.Collections.ObjectModel;

using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class OnlineRechargePageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        public List<string> PaymentMethods { get; } = new()
        {
            AppResource.DebitCard,
            AppResource.CreditCard,
            AppResource.NetBanking,
            AppResource.Wallet
        };

        [ObservableProperty]
        private ObservableCollection<RechargeProviderOption> _providerOptions = new()
        {
            new RechargeProviderOption { Title = AppResource.EzBuzzProviderTitle, ProviderType = RechargeProviderType.EzBuzz },
            new RechargeProviderOption { Title = AppResource.CashfreeProviderTitle, ProviderType = RechargeProviderType.Cashfree }
        };

        [ObservableProperty]
        private RechargeProviderOption? _selectedProvider;

        [ObservableProperty]
        private ObservableCollection<PaymentOptionItem> _paymentOptions = new()
        {
            new PaymentOptionItem { Title = AppResource.UpiPaymentOption, OptionType = PaymentOptionType.Upi },
            new PaymentOptionItem { Title = AppResource.RuPayPaymentOption, OptionType = PaymentOptionType.RuPay },
            new PaymentOptionItem { Title = AppResource.Paytm, OptionType = PaymentOptionType.Paytm },
            new PaymentOptionItem { Title = AppResource.MastercardPaymentOption, OptionType = PaymentOptionType.Mastercard },
            new PaymentOptionItem { Title = AppResource.VisaPaymentOption, OptionType = PaymentOptionType.Visa }
        };

        [ObservableProperty]
        private PaymentOptionItem? _selectedPaymentOption;

        [ObservableProperty]
        private string? _amount;

        [ObservableProperty]
        private string? _selectedPaymentMethod;

        #endregion

        #region [ Methods ]

        private bool _isInitialized;

        private void InitializeData()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            SelectedProvider = ProviderOptions.FirstOrDefault();
            SelectedPaymentOption = PaymentOptions.FirstOrDefault();

            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(IsBusy))
                    ContinuePaymentCommand.NotifyCanExecuteChanged();
            };
        }

        private void UpdateSelectedProviderState()
        {
            foreach (var provider in ProviderOptions)
                provider.IsSelected = ReferenceEquals(provider, SelectedProvider);
        }

        private void UpdateSelectedPaymentOptionState()
        {
            foreach (var option in PaymentOptions)
                option.IsSelected = ReferenceEquals(option, SelectedPaymentOption);
        }

        private bool CanContinuePayment()
        {
            return !IsBusy
                && SelectedProvider is not null
                && SelectedPaymentOption is not null
                && !string.IsNullOrWhiteSpace(SelectedPaymentMethod)
                && decimal.TryParse(Amount, out var amount)
                && amount > 0;
        }

        #endregion

        #region [ Property Changed ]

        partial void OnSelectedProviderChanged(RechargeProviderOption? value)
        {
            UpdateSelectedProviderState();
        }

        partial void OnSelectedPaymentOptionChanged(PaymentOptionItem? value)
        {
            UpdateSelectedPaymentOptionState();
            ContinuePaymentCommand.NotifyCanExecuteChanged();
        }

        partial void OnAmountChanged(string? value)
        {
            ContinuePaymentCommand.NotifyCanExecuteChanged();
        }

        partial void OnSelectedPaymentMethodChanged(string? value)
        {
            ContinuePaymentCommand.NotifyCanExecuteChanged();
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private void SelectProvider(RechargeProviderOption? provider)
        {
            if (provider is null)
                return;

            SelectedProvider = provider;
        }

        [RelayCommand]
        private void SelectPaymentOption(PaymentOptionItem? option)
        {
            if (option is null)
                return;

            SelectedPaymentOption = option;
        }

        [RelayCommand(CanExecute = nameof(CanContinuePayment))]
        private async Task ContinuePayment()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await ShowAlertAsync(
                    string.Format(AppResource.RechargeProcessingMessageFormat, Amount, SelectedPaymentOption?.Title, SelectedProvider?.Title),
                    AlertType.Success);
            }
            catch (Exception ex)
            {
                HandleException(ex, "Unhandled exception during ContinuePayment()");
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion

        #region [ Override Methods ]

        public override Task LoadDataWhenNavigatedTo()
        {
            InitializeData();
            return base.LoadDataWhenNavigatedTo();
        }

        #endregion
    }
}
