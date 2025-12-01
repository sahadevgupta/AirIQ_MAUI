using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    private Flight _selectedAirline;

    [ObservableProperty]
    private FlightSearchRequest? _flightSearchRequest;

    [ObservableProperty]
    private ObservableCollection<Passenger>? _adultPassengers;


    [ObservableProperty]
    private ObservableCollection<Passenger>? _infantPassengers = new();

    [ObservableProperty]
    private ObservableCollection<string>? _passengerTitles = new ObservableCollection<string>
    {
        StringConstants.Mr,
        StringConstants.Mrs,
        StringConstants.Ms,
        StringConstants.Mstr,
        StringConstants.Miss
    };

    private int? adultCount;
    
    private double infantCount = 0;

    #endregion

    #region [ Methods & Service Calls ]

    private async Task InitializeData()
    {
        LoadingService.ShowLoading();

        try
        {
            adultCount = FlightSearchRequest?.Adult;
            AddInfantPassengerCommand.NotifyCanExecuteChanged();
        
        if (SelectedAirline != null)
        {


            AdultPassengers = new ObservableCollection<Passenger>();
            for (int i = 0; i < adultCount; i++)
            {
                var passenger = new Passenger
                {
                    Id = $"{nameof(PassengerType.Adult)} {i}",
                    PassengerType = PassengerType.Adult,
                    IsExpanded = i == 0 ? true : false
                };
                passenger.PropertyChanged += OnPassengerPropertyChanged;
                AdultPassengers.Add(passenger);
            }
        }
        }
        catch (Exception ex)
        {
            // Handle exceptions
        }
        LoadingService.HideLoading();

    }

    private void OnPassengerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Passenger item = (Passenger)sender!;
        if(e.PropertyName == "SelectedPassengerType")
        {
            if(item.SelectedPassengerType ==nameof(PassengerType.Child))
            {
                PassengerTitles = new ObservableCollection<string>
                {
                    StringConstants.Mstr,
                    StringConstants.Miss
                };
            }
            
        }
    }

    private bool CanAddInfant()
    {
        return InfantPassengers?.Count <= adultCount ;
    }


    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task Continue()
    {
        var bookingRequest = new TicketBookingRequest
        {
            TicketId = SelectedAirline.TicketId,
            TotalPax = (AdultPassengers?.Count + InfantPassengers?.Count)?.ToString(),
            Adult = AdultPassengers?.Where(x => x.PassengerType == PassengerType.Adult).Count().ToString(),
            Child = AdultPassengers?.Where(x => x.PassengerType == PassengerType.Child).Count().ToString(),
            Infant = InfantPassengers?.Count.ToString(),
            AdultInfo = new List<PassengerRequest>(),
            ChildInfo = new List<PassengerRequest>(),
            InfantInfo = new List<InfantInfoRequest>()
        };

        foreach(var adultInfo in AdultPassengers?.Where(x => x.PassengerType == PassengerType.Adult))
        {
            bookingRequest.AdultInfo.Add(new PassengerRequest
            {
                Title = adultInfo.Title,
                FirstName = adultInfo.FirstName,
                LastName = adultInfo.LastName
            });
        }

        foreach(var childInfo in AdultPassengers?.Where(x => x.PassengerType == PassengerType.Child))
        {
            bookingRequest.ChildInfo.Add(new PassengerRequest
            {
                Title = childInfo.Title,
                FirstName = childInfo.FirstName,
                LastName = childInfo.LastName
            });
        }

        foreach(var infantInfo in InfantPassengers)
        {
            bookingRequest.AdultInfo.Add(new PassengerRequest
            {
                Title = infantInfo.Title,
                FirstName = infantInfo.FirstName,
                LastName = infantInfo.LastName
            });
        }

        var navigationParams = new Dictionary<string, object?>
        {
            { NavigationParamConstants.SelectedFlight, SelectedAirline },
            { NavigationParamConstants.SelectedFlight, SelectedAirline }
        };

        await ShellNavigationService.Navigate<SummaryPage>();
    }

    [RelayCommand(CanExecute =nameof(CanAddInfant))]
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
