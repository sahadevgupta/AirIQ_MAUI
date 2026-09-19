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

namespace AirIQ.ViewModels;

public partial class SalesRecordPageViewModel(IViewModelParameters viewModelParameters,
    IOperationsService operationsService,
    IPublicFileSaverService publicFileSaverService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    const int pageSize = 20;
    const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    int page = 1;

    private List<SalesRecord> salesRecordsTemp = new();
    private bool isExportingSalesRecords;
    private bool isLoadingMoreSalesRecords;

    [ObservableProperty]
    private ObservableRangeCollection<SalesRecord> _salesRecords = new();

    [ObservableProperty]
    private string? _searchText;
    #endregion

    #region [ Methods & Service Calls ]

    private void FilterSalesRecords(string? searchKey = null)
    {
        IEnumerable<SalesRecord> filtered = salesRecordsTemp;

        if (!string.IsNullOrEmpty(searchKey))
            filtered = filtered.Where(x =>
                x.Prefix.ContainsIgnoreCase(searchKey) ||
                x.PNR.ContainsIgnoreCase(searchKey) ||
                x.FDestName.ContainsIgnoreCase(searchKey) ||
                x.AirlineName.ContainsIgnoreCase(searchKey) ||
                x.PassengersName.ContainsIgnoreCase(searchKey));

        if (FromDate.HasValue)
            filtered = filtered.Where(x => x.EntryDate.Date >= FromDate.Value.Date);

        if (ToDate.HasValue)
            filtered = filtered.Where(x => x.EntryDate.Date <= ToDate.Value.Date);

        SalesRecords.ReplaceRange(filtered);
    }

    partial void OnSearchTextChanged(string? value)
    {
        if (string.IsNullOrEmpty(value))
            FilterSalesRecords(value);
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task LoadMoreAsync()
    {
        if (isLoadingMoreSalesRecords)
            return;

        isLoadingMoreSalesRecords = true;
        try
        {
            using (LoadingService.Show())
            {
                var records = await operationsService.GetSalesRecordsAsync(AppConfiguration.CurrentUser?.AgencyId ?? 0, page, pageSize);
                if (records.Any())
                {
                    var item = BackendToAppModelMapper.GetSalesRecords(records).ToList();
                    salesRecordsTemp.AddRange(item);

                    if (string.IsNullOrEmpty(SearchText))
                        SalesRecords?.AddRange(item);
                    else
                        FilterSalesRecords(SearchText);
                }
                page++;
            }
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        finally
        {
            isLoadingMoreSalesRecords = false;
        }
    }

    [RelayCommand]
    private void Search(string? searchText)
    {
        FilterSalesRecords(searchText);
    }

    [RelayCommand]
    private async Task DownloadAsync()
    {
        if (isExportingSalesRecords)
            return;

        if (!SalesRecords.Any())
        {
            ShowToast(AppResource.NoSalesRecordsToExport);
            return;
        }

        isExportingSalesRecords = true;
        try
        {
            using (LoadingService.Show())
            {
                var recordsToExport = SalesRecords.ToList();
                var fileBytes = await Task.Run(() => SalesRecordExcelExporter.Export(recordsToExport));
                var fileName = $"SalesRecords_{DateTime.Now:yyyy-MM-dd_HHmmss}.xlsx";

                try
                {
                    // Best-effort: a persistent, user-browsable copy in the device's
                    // Downloads (Android) / Files app (iOS) folder. The share step below
                    // still lets the user open/send the file even if this fails.
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
                    Title = AppResource.SalesRecord,
                    File = new ShareFile(filePath)
                });
            }

            ShowToast(AppResource.SalesRecordsExportedSuccessfully);
        }
        catch (Exception exception)
        {
            HandleException(exception);
        }
        finally
        {
            isExportingSalesRecords = false;
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
        FilterSalesRecords();
        return Task.CompletedTask;
    }

    #endregion
}