using System;
using System.Collections.ObjectModel;
using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

[QueryProperty(nameof(SelectedAirline), NavigationParamConstants.SelectedFlight)]
public partial class FlightBookingPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private Flight? _selectedAirline;

    [ObservableProperty]
    private ObservableCollection<Passenger>? _adultPassengers;


    [ObservableProperty]
    private ObservableCollection<Passenger>? _infantPassengers;

    [ObservableProperty]
    private double _totalPrice;

    [ObservableProperty]
    private string? _travellersCountText;

    private double adultCount = 3;
    private double infantCount =1;

    #endregion

    #region [ Methods & Service Calls ]

    private void InitializeData()
    {
        if (SelectedAirline != null)
        {


            AdultPassengers = new ObservableCollection<Passenger>();
            for (int i = 0; i < adultCount; i++)
            {
                AdultPassengers.Add(new Passenger { Id = $"{nameof(PassengerType.Adult)} {i}", PassengerType = PassengerType.Adult });
            }

            InfantPassengers = new ObservableCollection<Passenger>();
            for (int i = 0; i < infantCount; i++)
            {
                InfantPassengers.Add(new Passenger { Id = $"{nameof(PassengerType.Infant)} {i}", PassengerType = PassengerType.Infant });
            }
            
            TravellersCountText = $"{AdultPassengers.Count} Adults, {InfantPassengers.Count} Infants";



            TotalPrice = SelectedAirline.Price;
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

    #endregion

    #region [ Overrides ]

    public override Task LoadDataWhenNavigatedTo()
    {
        InitializeData();
        return Task.CompletedTask;
    }

    #endregion

}
