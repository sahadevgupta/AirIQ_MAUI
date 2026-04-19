using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Models.Request;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AirIQ.ViewModels;

[QueryProperty(nameof(SelectedAirline), NavigationParamConstants.SelectedFlight)]
[QueryProperty(nameof(FlightSearchRequest), NavigationParamConstants.FlightSearchRequest)]
public partial class FlightBookingPageViewModel(IViewModelParameters viewModelParameters, IFlightService flightService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private Flight? _selectedAirline;

    [ObservableProperty]
    private FlightSearchRequest? _flightSearchRequest;

    [ObservableProperty]
    private ObservableCollection<Passenger>? _passengers;

    [ObservableProperty]
    private ObservableCollection<string>? _infantTravelPartners = new();

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

    private int infantId = 1;

    #endregion

    #region [ Methods & Service Calls ]

    private async Task InitializeData()
    {

        try
        {
            using (LoadingService.Show())
            {
                adultCount = FlightSearchRequest?.Adult;
                AddInfantPassengerCommand.NotifyCanExecuteChanged();

                if (SelectedAirline != null)
                {
                    var tempPassengers = new List<Passenger>();
                    for (int i = 1; i <= adultCount; i++)
                    {
                        var passenger = new Passenger
                        {
                            Id = i,
                            Header = $"{PassengerType.Adult} #{i}",
                            Type = PassengerType.Adult,
                            IsSectionOpen = i == 1 ? true : false
                        };
                        passenger.PropertyChanged += OnPassengerPropertyChanged;
                        tempPassengers.Add(passenger);
                    }

                    Passengers = new ObservableCollection<Passenger>(tempPassengers);

                    InfantTravelPartners = new ObservableCollection<string>(tempPassengers.Where(p => p.Type == PassengerType.Adult)
                                                                                          .Select(a => a.Header.Replace("Passenger", "Adult")));

                    AddInfantPassengerCommand.NotifyCanExecuteChanged();

                }
            }
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

    }

    private void OnPassengerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Passenger item = (Passenger)sender!;
        if (e.PropertyName == "SelectedPassengerType")
        {
            if (item.SelectedPassengerType == nameof(PassengerType.Child))
            {
                PassengerTitles = new ObservableCollection<string>
                {
                    StringConstants.Mstr,
                    StringConstants.Miss
                };
            }

        }
    }

    public string? AssignInfantToAdult(Infant infant, Passenger adult)
    {
        string? infantTravelPartners = string.Empty;
        // 1️⃣ Adult already has an infant → Not allowed
        if (adult.AssignedInfant != null)
        {
            return $"Adult {adult.Header} already has one infant assigned.";
        }

        // 2️⃣ Infant already assigned → Not allowed
        if (infant.AssignedAdultId < 0)
        {
            return $"Infant {infant.Header} is already assigned to an adult.";
        }

        // 3️⃣ Assignment is valid → Assign both sides
        adult.AssignedInfant = infant;
        infant.AssignedAdultId = adult.Id;
        infantTravelPartners = adult.Header?.Replace("Passenger", "Adult");

        return infantTravelPartners; // success
    }

    private bool CanAddInfant()
    {
        var infantPassengers = Passengers?.Where(p => p.Type == PassengerType.Infant);
        return infantPassengers?.Count() < adultCount;
    }


    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void CanSaveExecute(object arg)
    {
        try
        {
            var passenger = arg as Passenger;
            passenger?.IsCompleted = !string.IsNullOrEmpty(passenger.SelectedPassengerType) &&
                                     !string.IsNullOrEmpty(passenger.SelectedTitle) &&
                                     !string.IsNullOrEmpty(passenger.FirstName) &&
                                     !string.IsNullOrEmpty(passenger.LastName);

        }
        catch (Exception exception)
        {
            HandleException(exception);
        }

    }

    [RelayCommand]
    private async Task ConfirmBooking()
    {
        using (LoadingService.Show())
        {


            var bookingRequest = new TicketBookingRequest
            {
                TicketId = SelectedAirline?.TicketId,
                TotalPax = Passengers?.Count.ToString(),
                Adult = Passengers?.Where(x => x.Type == PassengerType.Adult).Count().ToString(),
                Child = Passengers?.Where(x => x.Type == PassengerType.Child).Count().ToString(),
                Infant = Passengers?.Count.ToString(),
                AdultInfo = new List<PassengerRequest>(),
                ChildInfo = new List<PassengerRequest>(),
                InfantInfo = new List<InfantInfoRequest>()
            };

            foreach (var adultInfo in Passengers?.Where(x => x.Type == PassengerType.Adult))
            {
                bookingRequest.AdultInfo.Add(new PassengerRequest
                {
                    Title = adultInfo.Title,
                    FirstName = adultInfo.FirstName,
                    LastName = adultInfo.LastName
                });
            }

            foreach (var childInfo in Passengers?.Where(x => x.Type == PassengerType.Child))
            {
                bookingRequest.ChildInfo.Add(new PassengerRequest
                {
                    Title = childInfo.Title,
                    FirstName = childInfo.FirstName,
                    LastName = childInfo.LastName
                });
            }

            foreach (var infantInfo in Passengers?.Where(x => x.Type == PassengerType.Infant))
            {
                bookingRequest.AdultInfo.Add(new PassengerRequest
                {
                    Title = infantInfo.Title,
                    FirstName = infantInfo.FirstName,
                    LastName = infantInfo.LastName
                });
            }

            await flightService.ConfirmBookingAsync(bookingRequest);

        }
        // var navigationParams = new Dictionary<string, object?>
        // {
        //     { NavigationParamConstants.SelectedFlight, SelectedAirline },
        //     { NavigationParamConstants.SelectedFlight, SelectedAirline }
        // };

        // await ShellNavigationService.Navigate<SummaryPage>();
    }

    [RelayCommand(CanExecute = nameof(CanAddInfant))]
    private void AddInfantPassenger()
    {

        var newInfant = new Infant
        {
            Id = infantId++,
            Header = $"{nameof(PassengerType.Infant)} #{Passengers.Count}",
            Type = PassengerType.Infant,
        };

        newInfant.TravelWith = AssignInfantToAdult(newInfant, Passengers[newInfant.Id - 1]);
        Passengers?.Add(newInfant);
        AddInfantPassengerCommand.NotifyCanExecuteChanged();
    }

    #endregion

    #region [ Overrides ]

    public override async Task LoadDataWhenNavigatedTo()
    {
        _ = InitializeData();
    }

    #endregion

}
