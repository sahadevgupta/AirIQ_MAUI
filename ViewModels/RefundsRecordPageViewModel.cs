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
    public partial class RefundsRecordPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        const int pageSize = 20;
        int page = 1;

        private List<RefundRecord> refundRecordsTemp = new();

        [ObservableProperty]
        private ObservableRangeCollection<RefundRecord> _refundRecords = new();

        [ObservableProperty]
        private string? _searchText;
        #endregion

        #region [ Methods & Service Calls ]

        private void FilterRefundRecords(string? searchKey)
        {
            var filtered = string.IsNullOrEmpty(searchKey)
                ? refundRecordsTemp
                : refundRecordsTemp.Where(x =>
                    x.Prefix.ContainsIgnoreCase(searchKey) ||
                    x.PNR.ContainsIgnoreCase(searchKey) ||
                    x.FDestName.ContainsIgnoreCase(searchKey));

            RefundRecords.ReplaceRange(filtered);
        }

        partial void OnSearchTextChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
                FilterRefundRecords(value);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task LoadMoreAsync()
        {
            using (LoadingService.Show())
            {
                var records = await operationsService.GetRefundRecordsAsync(AppConfiguration.CurrentUser?.AgencyId ?? 0, page, pageSize);
                if (records.Any())
                {
                    var item = BackendToAppModelMapper.GetRefundRecords(records).ToList();
                    refundRecordsTemp.AddRange(item);

                    if (string.IsNullOrEmpty(SearchText))
                        RefundRecords?.AddRange(item);
                    else
                        FilterRefundRecords(SearchText);
                }
                page++;
            }
        }

        [RelayCommand]
        private void Search(string? searchText)
        {
            FilterRefundRecords(searchText);
        }

        #endregion

        #region [ Override Methods ]

        public override async Task LoadDataWhenNavigatedTo()
        {
            await LoadMoreAsync();
        }

        #endregion
    }
}