using System.Collections.ObjectModel;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class DashboardPageViewModel(IViewModelParameters viewModelParameters,
        IFlightService flightService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]
        private IEnumerable<FlightRoute>? Airports;
        private List<FlightRoute>? sourceTemp;
        private List<FlightRoute>? destTemp;

        [ObservableProperty]
        private ObservableCollection<FlightRoute>? _sourceAirports;

        [ObservableProperty]
        private ObservableCollection<FlightRoute>? _destinationAirports;

        [ObservableProperty]
        private FlightRoute? _selectedSourceAirport;

        [ObservableProperty]
        private FlightRoute? _selectedDestinationAirport;

        [ObservableProperty]
        private ObservableCollection<DateTime> _allowedDates = new();

        [ObservableProperty]
        private DateTime? _selectedTravelDate;

        [ObservableProperty]
        private int _paxSize = 1;

        #endregion

        #region [ Methods & Service Calls ]

        partial void OnSelectedSourceAirportChanged(FlightRoute? oldValue, FlightRoute? newValue)
        {
            SelectedDestinationAirport = null;
            SelectedTravelDate = null;
            GetDestinationAirports();
        }

        partial void OnSelectedDestinationAirportChanged(FlightRoute? oldValue, FlightRoute? newValue)
        {
            SelectedTravelDate = null;
            if (!string.IsNullOrWhiteSpace(SelectedSourceAirport?.Origin) && !string.IsNullOrWhiteSpace(SelectedDestinationAirport?.Destination))
                _ = GetAvailableBookingDatesAsync();
        }

        private async Task GetAvailableBookingDatesAsync()
        {
            var dates = await flightService.GetAvailableBookingDatesAsync(SelectedSourceAirport?.Origin!, SelectedDestinationAirport?.Destination!);
            AllowedDates = new ObservableCollection<DateTime>(dates);
        }

        public async Task InitializeDataAsync()
        {
            try
            {
                using (LoadingService.Show())
                {
                    var result = await flightService.GetAvailableRoutesAsync();

                    Airports = BackendToAppModelMapper.GetAvailableRoutes(result);
                    SourceAirports = new ObservableCollection<FlightRoute>(Airports.Where(x => !string.IsNullOrEmpty(x.Origin))
                                                                                    .GroupBy(x => x.Origin)
                                                                                    .Select(g => g.First()));

                    sourceTemp = new List<FlightRoute>(SourceAirports);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void GetDestinationAirports()
        {
            DestinationAirports = new ObservableCollection<FlightRoute>(Airports!.Where(x => x.Origin == SelectedSourceAirport?.Origin && !string.IsNullOrEmpty(x.Destination))
                                                                           .Distinct());
            destTemp = new List<FlightRoute>(DestinationAirports);
        }

        private void FilterListByQuery(string searchKey, string param)
        {
            if (param.Equals("destination"))
            {
                if (!string.IsNullOrEmpty(searchKey))
                {
                    DestinationAirports = new ObservableCollection<FlightRoute>(destTemp?.Where(x => x.DestinationRoute.Contains(searchKey.ToLower(), StringComparison.OrdinalIgnoreCase))!);
                }
                else
                {
                    DestinationAirports = new ObservableCollection<FlightRoute>(destTemp!);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchKey))
                {
                    SourceAirports = new ObservableCollection<FlightRoute>(sourceTemp?.Where(x => x.OriginRoute.Contains(searchKey.ToLower(), StringComparison.OrdinalIgnoreCase))!);
                }
                else
                {
                    SourceAirports = new ObservableCollection<FlightRoute>(sourceTemp!);
                }
            }
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private void OpenMenu()
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        [RelayCommand]
        private void SearchSourceAirports(string searchKey)
        {
            FilterListByQuery(searchKey, "source");
        }

        [RelayCommand]
        private void SearchDestinationAirports(string searchKey)
        {
            if (destTemp != null && destTemp.Any())
                FilterListByQuery(searchKey, "destination");
        }

        [RelayCommand]
        private async Task SearchFlights()
        {
            if (!string.IsNullOrWhiteSpace(SelectedSourceAirport?.Origin) &&
                !string.IsNullOrWhiteSpace(SelectedDestinationAirport?.Destination) &&
                SelectedTravelDate != null &&
                PaxSize > 0)
            {

                var request = new Models.Request.FlightSearchRequest
                {
                    Origin = SelectedSourceAirport?.Origin,
                    Destination = SelectedDestinationAirport?.Destination,
                    DepartureDate = SelectedTravelDate != null ?
                                    SelectedTravelDate.Value.ToString("yyyy/MM/dd") :
                                    string.Empty,
                    Adult = PaxSize,
                    SourceAirport = SelectedSourceAirport,
                    DestinationAirport = SelectedDestinationAirport,
                    OriginAirportName = SelectedSourceAirport?.OriginAiportName,
                    DestinationAirportName = SelectedDestinationAirport?.DestinationAiportName,
                    Child = 0,
                    Infant = 0,
                    AirlineCode = null
                };

                await ShellNavigationService.Navigate<FlightsPage>(parameters: new Dictionary<string, object>
                {
                    { NavigationParamConstants.FlightSearchRequest, request },
                    { NavigationParamConstants.TravelAllowedDates, AllowedDates },
                });

            }
        }

        [RelayCommand]
        private void SwapSourceDestination()
        {
            var temp = SelectedSourceAirport;
            SelectedSourceAirport = SelectedDestinationAirport;
            SelectedDestinationAirport = temp;
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
