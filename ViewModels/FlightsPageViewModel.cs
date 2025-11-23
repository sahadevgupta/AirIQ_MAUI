using System;
using System.Collections.ObjectModel;
using AirIQ.Configurations.Mapper;
using AirIQ.Constants;
using AirIQ.Models;
using AirIQ.Models.Request;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

[QueryProperty(nameof(FlightSearchRequest), NavigationParamConstants.FlightSearchRequest)]
public partial class FlightsPageViewModel(IViewModelParameters viewModelParameters, IFlightService flightService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private FlightSearchRequest? _flightSearchRequest;

    [ObservableProperty]
    private ObservableCollection<Flight>? _availableFlights;

    #endregion

    #region [ Methods & Service Calls ]

    private async Task IniatializeDataAsync()
    {
        try
        {
            LoadingService.ShowLoading();

            var response = await flightService.GetFlightAvailabilityAsync(FlightSearchRequest!);
            LoadingService.HideLoading();
            AvailableFlights = new ObservableCollection<Flight>(BackendToAppModelMapper.GetFlights(response));

        }
        catch (Exception ex)
        {
            // Handle exceptions
        }
        finally
        {
            LoadingService.HideLoading();
        }
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task NavigateToBookFlightView(Flight selectedFlight)
    {
        await ShellNavigationService.Navigate<FlightBookingPage>(parameters: new Dictionary<string, object>
        {
            { NavigationParamConstants.SelectedFlight, selectedFlight! }
        });

    }
    #endregion

    #region [ Overrides ]

    public override async Task LoadDataWhenNavigatedTo()
    {
        await IniatializeDataAsync();
    }

    #endregion
}
