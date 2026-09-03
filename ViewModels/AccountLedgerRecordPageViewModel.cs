using AirIQ.Configurations;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Extensions;
using AirIQ.Helpers;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class AccountLedgerRecordPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        const int pageSize = 20;
        int page = 1;

        private List<AccountLedgerRecord> accountLedgerRecordsTemp = new();

        [ObservableProperty]
        private ObservableRangeCollection<AccountLedgerRecord> _accountLedgerRecords = new();

        [ObservableProperty]
        private string? _searchText;
        #endregion

        #region [ Methods & Service Calls ]

        private void FilterAccountLedgerRecords(string? searchKey)
        {
            var filtered = string.IsNullOrEmpty(searchKey)
                ? accountLedgerRecordsTemp
                : accountLedgerRecordsTemp.Where(x =>
                    x.RefNo.ContainsIgnoreCase(searchKey) ||
                    x.Particulars.ContainsIgnoreCase(searchKey) ||
                    x.Destination.ContainsIgnoreCase(searchKey));

            AccountLedgerRecords.ReplaceRange(filtered);
        }

        partial void OnSearchTextChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
                FilterAccountLedgerRecords(value);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task LoadMoreAsync()
        {
            try
            {
                using (LoadingService.Show())
                {
                    var records = await operationsService.GetAccountLedgerRecordsAsync(AppConfiguration.CurrentUser?.AgencyId ?? 0, page, pageSize);
                    if (records.Any())
                    {
                        var item = BackendToAppModelMapper.GetAccountLedgerRecords(records).ToList();
                        accountLedgerRecordsTemp.AddRange(item);

                        if (string.IsNullOrEmpty(SearchText))
                            AccountLedgerRecords?.AddRange(item);
                        else
                            FilterAccountLedgerRecords(SearchText);
                    }
                    page++;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        [RelayCommand]
        private void Search(string? searchText)
        {
            FilterAccountLedgerRecords(searchText);
        }

        #endregion

        #region [ Override Methods ]

        public override async Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
        {
            await LoadMoreAsync();
        }

        #endregion
    }
}
