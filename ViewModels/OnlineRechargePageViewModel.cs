using System.Collections.ObjectModel;

using AirIQ.Enums;
using AirIQ.Models;
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
            "Debit Card",
            "Credit Card",
            "Net Banking",
            "Wallet"
        };

        [ObservableProperty]
        private ObservableCollection<RechargeProviderOption> _providerOptions = new()
        {
            new RechargeProviderOption { Title = "EZBUZZ", ProviderType = RechargeProviderType.EzBuzz },
            new RechargeProviderOption { Title = "CASHFREE", ProviderType = RechargeProviderType.Cashfree }
        };

        [ObservableProperty]
        private RechargeProviderOption? _selectedProvider;

        [ObservableProperty]
        private ObservableCollection<PaymentOptionItem> _paymentOptions = new()
        {
            new PaymentOptionItem { Title = "UPI", OptionType = PaymentOptionType.Upi },
            new PaymentOptionItem { Title = "RuPay", OptionType = PaymentOptionType.RuPay },
            new PaymentOptionItem { Title = "Paytm", OptionType = PaymentOptionType.Paytm },
            new PaymentOptionItem { Title = "Mastercard", OptionType = PaymentOptionType.Mastercard },
            new PaymentOptionItem { Title = "Visa", OptionType = PaymentOptionType.Visa }
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
                    $"Recharge of ₹{Amount} via {SelectedPaymentOption?.Title} will be processed through {SelectedProvider?.Title}.",
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
