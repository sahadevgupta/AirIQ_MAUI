using System.Collections.ObjectModel;

using AirIQ.Configurations;
using AirIQ.Enums;
using AirIQ.Extensions;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class WalletPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
        [ObservableProperty]
        private string _walletId = "WLT782913";

        [ObservableProperty]
        private double _totalCredits;

        [ObservableProperty]
        private double _totalDebits;

        [ObservableProperty]
        private bool _isBalanceVisible = true;

        [ObservableProperty]
        private ObservableCollection<WalletTransaction> _recentTransactions = [];

        public string BalanceDisplayText => IsBalanceVisible ? $"₹ {CurrentUser.Balance:N2}" : "₹ ••••••";

        public string EyeIconGlyph => IsBalanceVisible ? FontAwesomeIcons.EyeSlash : FontAwesomeIcons.Eye;

        public override Task LoadDataWhenOnAppearing(CancellationToken cancellationToken = default)
        {
            LoadWalletSummary();
            return base.LoadDataWhenOnAppearing(cancellationToken);
        }

        private void LoadWalletSummary()
        {
            RecentTransactions =
            [
                new WalletTransaction { Title = AppResource.AddMoney, Date = new DateTime(2026, 9, 1, 9, 15, 0), Amount = 5000, Balance = 12450.75, Type = WalletTransactionType.Credit, IconGlyph = FontAwesomeIcons.ArrowDown },
                new WalletTransaction { Title = "Hotel Booking", Date = new DateTime(2026, 8, 30, 19, 42, 0), Amount = 2150, Balance = 7450.75, Type = WalletTransactionType.Debit, IconGlyph = FontAwesomeIcons.Bed },
                new WalletTransaction { Title = "Flight Booking", Date = new DateTime(2026, 8, 28, 11, 30, 0), Amount = 3999, Balance = 9600.75, Type = WalletTransactionType.Debit, IconGlyph = FontAwesomeIcons.Plane },
                new WalletTransaction { Title = AppResource.AddMoney, Date = new DateTime(2026, 8, 25, 14, 10, 0), Amount = 3500, Balance = 13599.75, Type = WalletTransactionType.Credit, IconGlyph = FontAwesomeIcons.ArrowDown },
                new WalletTransaction { Title = "Inspection Fee", Date = new DateTime(2026, 8, 24, 10, 5, 0), Amount = 1200, Balance = 10099.75, Type = WalletTransactionType.Debit, IconGlyph = FontAwesomeIcons.ClipboardCheck },
            ];

            TotalCredits = RecentTransactions.Where(t => t.IsCredit).Sum(t => t.Amount);
            TotalDebits = RecentTransactions.Where(t => !t.IsCredit).Sum(t => t.Amount);
        }

        partial void OnIsBalanceVisibleChanged(bool value)
        {
            OnPropertyChanged(nameof(BalanceDisplayText));
            OnPropertyChanged(nameof(EyeIconGlyph));
        }

        #region [ Commands ]

        [RelayCommand]
        private void ToggleBalanceVisibility()
        {
            IsBalanceVisible = !IsBalanceVisible;
        }

        [RelayCommand]
        private async Task CopyWalletId()
        {
            await Clipboard.Default.SetTextAsync(WalletId);
            ShowToast(AppResource.WalletIdCopiedMessage);
        }

        [RelayCommand]
        private void NotAvailable()
        {
            ShowToast(AppResource.FeatureComingSoon);
        }

        #endregion
    }
}
