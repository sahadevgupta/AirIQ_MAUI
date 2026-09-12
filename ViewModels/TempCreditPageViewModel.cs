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
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class TempCreditPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        const int pageSize = 20;
        int page = 1;

        private List<TempCreditRecord> tempCreditRecordsTemp = new();

        [ObservableProperty]
        private ObservableRangeCollection<TempCreditRecord> _tempCreditRecords = new();

        [ObservableProperty]
        private string? _searchText;

        public double TotalAmount => TempCreditRecords.Sum(x => x.Amount);

        #endregion

        #region [ Methods & Service Calls ]

        private void FilterTempCreditRecords(string? searchKey)
        {
            var filtered = string.IsNullOrEmpty(searchKey)
                ? tempCreditRecordsTemp
                : tempCreditRecordsTemp.Where(x => x.Name.ContainsIgnoreCase(searchKey));

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

        #endregion

        #region [ Override Methods ]

        public override async Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
        {
            await LoadMoreAsync();
        }

        #endregion
    }
}