using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public partial class TempCreditPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService,
        IPublicFileSaverService publicFileSaverService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        const int pageSize = 20;
        const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        int page = 1;

        private List<TempCreditRecord> tempCreditRecordsTemp = new();
        private bool isExportingTempCreditRecords;

        [ObservableProperty]
        private ObservableRangeCollection<TempCreditRecord> _tempCreditRecords = new();

        [ObservableProperty]
        private string? _searchText;

        public double TotalAmount => TempCreditRecords.Sum(x => x.Amount);

        #endregion

        #region [ Methods & Service Calls ]

        private void FilterTempCreditRecords(string? searchKey)
        {
            IEnumerable<TempCreditRecord> filtered = tempCreditRecordsTemp;

            if (!string.IsNullOrEmpty(searchKey))
                filtered = filtered.Where(x => x.Name.ContainsIgnoreCase(searchKey));

            if (FromDate.HasValue)
                filtered = filtered.Where(x => x.Date.Date >= FromDate.Value.Date);

            if (ToDate.HasValue)
                filtered = filtered.Where(x => x.Date.Date <= ToDate.Value.Date);

            TempCreditRecords.ReplaceRange(filtered);
            OnPropertyChanged(nameof(TotalAmount));
        }

        partial void OnSearchTextChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
                FilterTempCreditRecords(value);
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
                    var records = await operationsService.GetTempCreditRecordsAsync(AppConfiguration.CurrentUser?.AgencyId ?? 0, page, pageSize);
                    if (records.Any())
                    {
                        var item = BackendToAppModelMapper.GetTempCreditRecords(records).ToList();
                        tempCreditRecordsTemp.AddRange(item);

                        if (string.IsNullOrEmpty(SearchText))
                        {
                            TempCreditRecords?.AddRange(item);
                            OnPropertyChanged(nameof(TotalAmount));
                        }
                        else
                        {
                            FilterTempCreditRecords(SearchText);
                        }
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
            FilterTempCreditRecords(searchText);
        }

        [RelayCommand]
        private async Task DownloadAsync()
        {
            if (isExportingTempCreditRecords)
                return;

            if (!TempCreditRecords.Any())
            {
                ShowToast(AppResource.NoTempCreditRecordsToExport);
                return;
            }

            isExportingTempCreditRecords = true;
            try
            {
                using (LoadingService.Show())
                {
                    var recordsToExport = TempCreditRecords.ToList();
                    var fileBytes = await Task.Run(() => TempCreditRecordExcelExporter.Export(recordsToExport));
                    var fileName = $"TempCredit_{DateTime.Now:yyyy-MM-dd_HHmmss}.xlsx";

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
                        Title = AppResource.SalesTemporaryCredit,
                        File = new ShareFile(filePath)
                    });
                }

                ShowToast(AppResource.TempCreditRecordsExportedSuccessfully);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                isExportingTempCreditRecords = false;
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
            FilterTempCreditRecords(SearchText);
            return Task.CompletedTask;
        }

        #endregion
    }
}