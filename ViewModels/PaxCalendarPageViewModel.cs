using System.Collections.ObjectModel;

using AirIQ.Constants;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.ViewModels
{
    public partial class PaxCalendarPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        [ObservableProperty]
        private DateTime _currentDate = DateTime.Today;

        [ObservableProperty]
        private DateTime _selectedTravelDate;

        [ObservableProperty]
        private ObservableCollection<PaxCalendarFlightDto>? _flightBookingList;

        #endregion

        #region [ Methods & Service Calls ]

        private async Task InitializeDataAsync()
        {
            using (LoadingService.Show())
            {
                var result = await operationsService.GetPaxCalendarFlightAsync(AppConfiguration.CurrentUser?.AgencyId ?? 0, SelectedTravelDate.ToString("yyyy-MM-dd"));
                FlightBookingList = new ObservableCollection<PaxCalendarFlightDto>(result);
            }
        }

        #endregion

        #region [ Override Methods ]
        public override async Task LoadDataWhenNavigatedTo()
        {
            await InitializeDataAsync();
        }

        #endregion
    }
}