using System;
using System.Collections.ObjectModel;
using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Models.Request;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

[QueryProperty(nameof(SelectedAirline), NavigationParamConstants.SelectedFlight)]
[QueryProperty(nameof(FlightSearchRequest), NavigationParamConstants.FlightSearchRequest)]
public partial class FlightBookingPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private Flight? _selectedAirline;

    [ObservableProperty]
    private FlightSearchRequest? _flightSearchRequest;

    [ObservableProperty]
    private ObservableCollection<Passenger>? _adultPassengers;


    [ObservableProperty]
    private ObservableCollection<Passenger>? _infantPassengers;

    [ObservableProperty]
    private string? _selectedPassengerType = nameof(PassengerType.Adult);

    [ObservableProperty]
    private string? _selectedTitle = "Mr.";

    [ObservableProperty]
    private double _totalPrice;

    [ObservableProperty]
    private string? _travellersCountText;

    private int? adultCount => FlightSearchRequest?.Adult;
    private double infantCount = 0;

    #endregion

    #region [ Methods & Service Calls ]

    private async Task InitializeData()
    {
        LoadingService.ShowLoading();
        
        if (SelectedAirline != null)
        {


            AdultPassengers = new ObservableCollection<Passenger>();
            for (int i = 0; i < adultCount; i++)
            {
                AdultPassengers.Add(new Passenger
                {
                    Id = $"{nameof(PassengerType.Adult)} {i}",
                    PassengerType = PassengerType.Adult,
                    IsExpanded = i == 0 ? true : false
                });
            }

            InfantPassengers = new ObservableCollection<Passenger>();
            for (int i = 0; i < infantCount; i++)
            {
                InfantPassengers.Add(new Passenger { Id = $"{nameof(PassengerType.Infant)} {i}", PassengerType = PassengerType.Infant });
            }

            TravellersCountText = $"{AdultPassengers.Count} Adults, {InfantPassengers.Count} Infants";



            TotalPrice = SelectedAirline.Price;

            LoadingService.HideLoading();
        }
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task Continue()
    {
        var navigationParams = new Dictionary<string, object?>
        {
            { NavigationParamConstants.SelectedFlight, SelectedAirline }
        };

        await ShellNavigationService.Navigate<SummaryPage>();
    }

    [RelayCommand]
    private void AddInfantPassenger()
    {
        InfantPassengers ??= new ObservableCollection<Passenger>();

        var newInfant = new Passenger
        {
            Id = $"{nameof(PassengerType.Infant)} {InfantPassengers.Count}",
            PassengerType = PassengerType.Infant
        };

        InfantPassengers.Add(newInfant);

    }

    #endregion

    #region [ Overrides ]

    public override async Task LoadDataWhenNavigatedTo()
    {
        await InitializeData();
    }

    #endregion

}
