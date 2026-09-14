using AirIQ.Configurations;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Extensions;
using AirIQ.Helpers;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class RefundsRecordPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService,
        IPublicFileSaverService publicFileSaverService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        const int pageSize = 20;
        const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        int page = 1;

        private List<RefundRecord> refundRecordsTemp = new();
        private bool isExportingRefundRecords;

        [ObservableProperty]
        private ObservableRangeCollection<RefundRecord> _refundRecords = new();

        [ObservableProperty]
        private string? _searchText;
        #endregion

        #region [ Methods & Service Calls ]

        private void FilterRefundRecords(string? searchKey)
        {
            IEnumerable<RefundRecord> filtered = refundRecordsTemp;

            if (!string.IsNullOrEmpty(searchKey))
                filtered = filtered.Where(x =>
                    x.Prefix.ContainsIgnoreCase(searchKey) ||
                    x.PNR.ContainsIgnoreCase(searchKey) ||
                    x.FDestName.ContainsIgnoreCase(searchKey));

            if (FromDate.HasValue)
                filtered = filtered.Where(x => x.EntryDate.Date >= FromDate.Value.Date);

            if (ToDate.HasValue)
                filtered = filtered.Where(x => x.EntryDate.Date <= ToDate.Value.Date);

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
            try
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
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        [RelayCommand]
        private void Search(string? searchText)
        {
            FilterRefundRecords(searchText);
        }

        [RelayCommand]
        private async Task DownloadAsync()
        {
            if (isExportingRefundRecords)
                return;

            if (!RefundRecords.Any())
            {
                ShowToast(AppResource.NoRefundRecordsToExport);
                return;
            }

            isExportingRefundRecords = true;
            try
            {
                using (LoadingService.Show())
                {
                    var recordsToExport = RefundRecords.ToList();
                    var fileBytes = await Task.Run(() => RefundRecordExcelExporter.Export(recordsToExport));
                    var fileName = $"Refunds_{DateTime.Now:yyyy-MM-dd_HHmmss}.xlsx";

                    try
                    {
                        await publicFileSaverService.SaveToDownloadsAsync(fileName, fileBytes, ExcelContentType);
                    }
                    catch (Exception saveException)
                    {
                        SentrySdk.CaptureException(saveException);
                    }

                    var filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
                    await File.WriteAllBytesAsync(filePath, fileBytes);

                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = AppResource.RefundTickets,
                        File = new ShareFile(filePath)
                    });
                }

                ShowToast(AppResource.RefundRecordsExportedSuccessfully);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                isExportingRefundRecords = false;
            }
        }

        #endregion

        #region [ Override Methods ]

        public override async Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
        {
            await LoadMoreAsync();
        }

        protected override Task OnDateFilterAppliedAsync(DateTime? fromDate, DateTime? toDate)
        {
            FilterRefundRecords(SearchText);
            return Task.CompletedTask;
        }

        #endregion
    }
}