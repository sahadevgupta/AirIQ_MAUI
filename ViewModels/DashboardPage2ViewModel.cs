using System.Collections.ObjectModel;
using System.Diagnostics;
using AirIQ.Configurations;
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
    [QueryProperty(nameof(SelectedTravelDateResult), NavigationParamConstants.SelectedTravelDateResult)]
    [QueryProperty(nameof(SelectedReturnDateResult), NavigationParamConstants.SelectedReturnDateResult)]
    public partial class DashboardPage2ViewModel(IViewModelParameters viewModelParameters,
        IFlightService flightService, TravelDatesPageViewModel travelDatesPageViewModel) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]
        private IEnumerable<FlightRoute>? Airports;

        [ObservableProperty]
        private AirportSelectionResult? _airportSelectionResult;

        [ObservableProperty]
        private DateTime? _selectedTravelDateResult;

        [ObservableProperty]
        private DateTime? _selectedReturnDateResult;

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
        private DateTime? _returnDate;

        [ObservableProperty]
        private int _paxSize = 1;

        [ObservableProperty]
        private int _adultCount = 1;

        [ObservableProperty]
        private int _childCount;

        [ObservableProperty]
        private int _infantCount;

        [ObservableProperty]
        private ObservableCollection<FlightRoute>? _popularDestinations;

        [ObservableProperty]
        private string? _amount;

        #endregion

        #region [ Methods & Service Calls ]

        partial void OnAdultCountChanged(int oldValue, int newValue) => PaxSize = AdultCount + ChildCount + InfantCount;

        partial void OnChildCountChanged(int oldValue, int newValue) => PaxSize = AdultCount + ChildCount + InfantCount;

        partial void OnInfantCountChanged(int oldValue, int newValue) => PaxSize = AdultCount + ChildCount + InfantCount;

        partial void OnSelectedSourceAirportChanged(FlightRoute? oldValue, FlightRoute? newValue)
        {
            SelectedDestinationAirport = null;
            SelectedTravelDate = null;
            ReturnDate = null;
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

        partial void OnSelectedTravelDateResultChanged(DateTime? value)
        {
            if (value is null)
                return;

            SelectedTravelDate = value;
            SelectedTravelDateResult = null;
        }

        partial void OnSelectedReturnDateResultChanged(DateTime? value)
        {
            if (value is null)
                return;

            ReturnDate = value;
            SelectedReturnDateResult = null;
        }

        partial void OnSelectedDestinationAirportChanged(FlightRoute? oldValue, FlightRoute? newValue)
        {
            SelectedTravelDate = null;
            ReturnDate = null;
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
            FormatAmount();
            travelDatesPageViewModel.Preload();

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

        private void FormatAmount()
        {
            var num = AppConfiguration.CurrentUser?.Balance ?? 0;
            if (num >= 1000)
            {
                Amount = $"{(num / 1000):F2}k";
                //control.amountSpan.Text = num.ToString("00.00,k");
            }
            else
                Amount = num.ToString("0.##");
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
        private async Task OpenDepartureDatePicker() => await OpenTravelDatesPicker(DateSelectionStage.Departure);

        // One-way only for now - round-trip entry points are commented out rather than removed, so
        // they can be re-enabled later alongside TravelDatesPageViewModel's round-trip logic. To bring
        // this back: uncomment these two commands, the ReturnDate.HasValue block in
        // OpenTravelDatesPicker below, and the Return-date row in DashboardPage2.xaml.
        // [RelayCommand]
        // private async Task OpenReturnDatePicker() => await OpenTravelDatesPicker(DateSelectionStage.Return);

        // [RelayCommand]
        // private void ClearReturnDate() => ReturnDate = null;

        private async Task OpenTravelDatesPicker(DateSelectionStage stage)
        {
            // Assigned directly on the pre-warmed singleton instance rather than round-tripped
            // through Shell navigation query parameters - see TravelDatesPageViewModel.Preload()/
            // PrepareForSelection().
            Debug.WriteLine("Clicked on Travel Date : " + DateTime.Now);
            travelDatesPageViewModel.PrepareForSelection(SelectedTravelDate, AllowedDates, stage);

            await ShellNavigationService.Navigate<TravelDatesPage>();
            Debug.WriteLine("Navigation to Travel Date completed : " + DateTime.Now);
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
                    ReturnDate = ReturnDate?.ToString("yyyy/MM/dd"),
                    Adult = AdultCount,
                    SourceAirport = SelectedSourceAirport,
                    DestinationAirport = SelectedDestinationAirport,
                    OriginAirportName = SelectedSourceAirport?.OriginAiportName,
                    DestinationAirportName = SelectedDestinationAirport?.DestinationAiportName,
                    Child = ChildCount,
                    Infant = InfantCount,
                    AirlineCode = null
                };

                await ShellNavigationService.Navigate<FlightsPage>(parameters: new Dictionary<string, object>
                {
                    { NavigationParamConstants.FlightSearchRequest, request },
                    { NavigationParamConstants.TravelAllowedDates, AllowedDates },
                });

            }
        }

        // Pure check (no mutation, no alert) so the view can decide whether to run the swap
        // animation/mutation *before* anything changes - see DashboardPage2.xaml.cs SwapButtonClicked.
        public bool IsReverseRouteAvailable()
        {
            var source = SelectedSourceAirport;
            var destination = SelectedDestinationAirport;

            if (source is null || destination is null)
                return true;

            if (string.IsNullOrWhiteSpace(source.Origin) || string.IsNullOrWhiteSpace(destination.Destination))
                return true;

            if (source.Origin == destination.Destination)
                return true;

            // Airports not loaded yet (or the initial load failed) - fail open rather than block
            // the user on a check we have no data for; GetAvailableRoutesAsync's own try/catch
            // already surfaces load failures via HandleException.
            if (Airports is null)
                return true;

            var a = Airports.FirstOrDefault(route => route.Origin == destination.Destination && route.Destination == source.Origin);
            return a != null;
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
