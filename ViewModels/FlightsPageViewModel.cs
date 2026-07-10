using System;
using System.Collections.ObjectModel;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Helpers;
using AirIQ.Models;
using AirIQ.Models.Request;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

[QueryProperty(nameof(FlightSearchRequest), NavigationParamConstants.FlightSearchRequest)]
[QueryProperty(nameof(AllowedDates), NavigationParamConstants.TravelAllowedDates)]
public partial class FlightsPageViewModel(IViewModelParameters viewModelParameters, IFlightService flightService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    private bool initialState = true;

    [ObservableProperty]
    private FlightSearchRequest? _flightSearchRequest;

    [ObservableProperty]
    private ObservableCollection<Flight> _availableFlights = new();

    [ObservableProperty]
    private ObservableCollection<DateTime> _allowedDates = new();

    [ObservableProperty]
    private DateTime _currentDate = DateTime.Today;

    [ObservableProperty]
    private DateTime _selectedTravelDate;

    [ObservableProperty]
    private string? _originCode;

    [ObservableProperty]
    private string? _destinationCode;

    [ObservableProperty]
    private string? _sourceAirport;

    [ObservableProperty]
    private string? _destinationAirport;

    #endregion

    #region [ Methods & Service Calls ]

    partial void OnCurrentDateChanged(DateTime value)
    {
        if (FlightSearchRequest != null)
        {
            FlightSearchRequest.DepartureDate = value.ToString("yyyy/MM/dd");
            _ = IniatializeDataAsync();

        }
    }

    partial void OnSelectedTravelDateChanged(DateTime value)
    {
        if (!initialState)
        {
            if (FlightSearchRequest != null)
            {
                FlightSearchRequest.DepartureDate = value.ToString("yyyy/MM/dd");
                _ = IniatializeDataAsync();

            }
        }
    }

    private async Task IniatializeDataAsync()
    {
        try
        {
            using (LoadingService.Show())
            {
                initialState = true;
                SelectedTravelDate = DateTime.Parse(FlightSearchRequest!.DepartureDate!);
                SourceAirport = FlightSearchRequest.SourceAirport?.OriginRoute;
                DestinationAirport = FlightSearchRequest.DestinationAirport?.DestinationRoute;
                var response = await flightService.GetFlightAvailabilityAsync(FlightSearchRequest!);
                AvailableFlights = new ObservableCollection<Flight>(BackendToAppModelMapper.GetFlights(response));
                initialState = false;
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task NavigateToBookFlightView(Flight selectedFlight)
    {
        await ShellNavigationService.Navigate<FlightBookingPage>(parameters: new Dictionary<string, object>
        {
            { NavigationParamConstants.SelectedFlight, selectedFlight! },
            { NavigationParamConstants.FlightSearchRequest, FlightSearchRequest! }
        });

    }

    [RelayCommand]
    private async Task CopyFlightDetail(Flight selectedFlight)
    {
        await ShowSnackBar("Details are copied", fontSize: ScalingHelper.ScaleFontSize(16));
    }

    #endregion

    #region [ Overrides ]

    public override async Task LoadDataWhenNavigatedTo()
    {
        await IniatializeDataAsync();
    }

    #endregion
}
