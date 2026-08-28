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

namespace AirIQ.ViewModels;

public partial class SalesRecordPageViewModel(IViewModelParameters viewModelParameters,
    IOperationsService operationsService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    const int pageSize = 20;
    int page = 1;

    private List<SalesRecord> salesRecordsTemp = new();

    [ObservableProperty]
    private ObservableRangeCollection<SalesRecord> _salesRecords = new();

    [ObservableProperty]
    private string? _searchText;
    #endregion

    #region [ Methods & Service Calls ]

    private void FilterSalesRecords(string? searchKey)
    {
        var filtered = string.IsNullOrEmpty(searchKey)
            ? salesRecordsTemp
            : salesRecordsTemp.Where(x =>
                x.Prefix.ContainsIgnoreCase(searchKey) ||
                x.PNR.ContainsIgnoreCase(searchKey) ||
                x.FDestName.ContainsIgnoreCase(searchKey) ||
                x.AirlineName.ContainsIgnoreCase(searchKey) ||
                x.PassengersName.ContainsIgnoreCase(searchKey));

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

    [RelayCommand]
    private void Search(string? searchText)
    {
        FilterSalesRecords(searchText);
    }

    #endregion

    #region [ Override Methods ]

    public override async Task LoadDataWhenNavigatedTo()
    {
        await LoadMoreAsync();
    }

    #endregion
}