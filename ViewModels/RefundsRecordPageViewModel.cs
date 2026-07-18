using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
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

        [ObservableProperty]
        private ObservableRangeCollection<RefundRecord> _refundRecords = new();
        #endregion

        #region [ Methods & Service Calls ]

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
                    RefundRecords?.AddRange(item);
                }
                page++;
            }
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