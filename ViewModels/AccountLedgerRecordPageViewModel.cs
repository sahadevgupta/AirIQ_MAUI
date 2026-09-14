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
    public partial class AccountLedgerRecordPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService,
        IPublicFileSaverService publicFileSaverService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        const int pageSize = 20;
        const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        int page = 1;

        private List<AccountLedgerRecord> accountLedgerRecordsTemp = new();
        private bool isExportingAccountLedgerRecords;

        [ObservableProperty]
        private ObservableRangeCollection<AccountLedgerRecord> _accountLedgerRecords = new();

        [ObservableProperty]
        private string? _searchText;
        #endregion

        #region [ Methods & Service Calls ]

        private void FilterAccountLedgerRecords(string? searchKey)
        {
            IEnumerable<AccountLedgerRecord> filtered = accountLedgerRecordsTemp;

            if (!string.IsNullOrEmpty(searchKey))
                filtered = filtered.Where(x =>
                    x.RefNo.ContainsIgnoreCase(searchKey) ||
                    x.Particulars.ContainsIgnoreCase(searchKey) ||
                    x.Destination.ContainsIgnoreCase(searchKey));

            if (FromDate.HasValue)
                filtered = filtered.Where(x => x.Date.Date >= FromDate.Value.Date);

            if (ToDate.HasValue)
                filtered = filtered.Where(x => x.Date.Date <= ToDate.Value.Date);

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

        [RelayCommand]
        private async Task DownloadAsync()
        {
            if (isExportingAccountLedgerRecords)
                return;

            if (!AccountLedgerRecords.Any())
            {
                ShowToast(AppResource.NoAccountLedgerRecordsToExport);
                return;
            }

            isExportingAccountLedgerRecords = true;
            try
            {
                using (LoadingService.Show())
                {
                    var recordsToExport = AccountLedgerRecords.ToList();
                    var fileBytes = await Task.Run(() => AccountLedgerRecordExcelExporter.Export(recordsToExport));
                    var fileName = $"AccountLedger_{DateTime.Now:yyyy-MM-dd_HHmmss}.xlsx";

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
                        Title = AppResource.AccountLedgerTitle,
                        File = new ShareFile(filePath)
                    });
                }

                ShowToast(AppResource.AccountLedgerRecordsExportedSuccessfully);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                isExportingAccountLedgerRecords = false;
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
            FilterAccountLedgerRecords(SearchText);
            return Task.CompletedTask;
        }

        #endregion
    }
}
