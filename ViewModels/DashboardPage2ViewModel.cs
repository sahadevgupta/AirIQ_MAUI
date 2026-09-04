using System.Collections.ObjectModel;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Helpers;
using AirIQ.Models;
using AirIQ.Popups;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Interfaces;

namespace AirIQ.ViewModels
{
    [QueryProperty(nameof(AirportSelectionResult), NavigationParamConstants.AirportSelectionResult)]
    public partial class DashboardPage2ViewModel(IViewModelParameters viewModelParameters,
        IFlightService flightService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]
        private IEnumerable<FlightRoute>? Airports;

        [ObservableProperty]
        private AirportSelectionResult? _airportSelectionResult;

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

        [ObservableProperty]
        private ObservableCollection<FlightRoute>? _popularDestinations;

        #endregion

        #region [ Methods & Service Calls ]

        partial void OnSelectedSourceAirportChanged(FlightRoute? oldValue, FlightRoute? newValue)
        {
            SelectedDestinationAirport = null;
            SelectedTravelDate = null;
            GetDestinationAirports();
        }

        partial void OnAirportSelectionResultChanged(AirportSelectionResult? value)
        {
            if (value?.SelectedAirport is null)
                return;

            if (value.FieldType == AirportFieldType.Source)
                SelectedSourceAirport = value.SelectedAirport;
            else
                SelectedDestinationAirport = value.SelectedAirport;

            AirportSelectionResult = null;
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

                    PopularDestinations = new ObservableCollection<FlightRoute>(Airports.Where(x => !string.IsNullOrEmpty(x.Destination))
                                                                                         .GroupBy(x => x.Destination)
                                                                                         .Select(g => g.First())
                                                                                         .Take(8));
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
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task OpenAirportSearch(AirportFieldType fieldType)
        {
            var airports = fieldType == AirportFieldType.Source ? SourceAirports : DestinationAirports;

            await ShellNavigationService.Navigate<AirportSearchPage>(parameters: new Dictionary<string, object>
            {
                { NavigationParamConstants.AirportFieldType, fieldType },
                { NavigationParamConstants.AirportList, airports ?? new ObservableCollection<FlightRoute>() },
            });
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

        [RelayCommand]
        private void SelectPopularDestination(FlightRoute route)
        {
            if (route is null)
                return;

            SelectedDestinationAirport = route;
        }

        [RelayCommand]
        private void ViewAllDestinations()
        {
            ShowToast(AirIQ.Resources.Strings.AppResource.FeatureComingSoon);
        }

        #endregion

        #region [ Override Methods ]

        public override async Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
        {
            await InitializeDataAsync();
        }

        #endregion

    }
}
