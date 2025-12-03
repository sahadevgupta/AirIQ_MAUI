using System;
using System.Collections.ObjectModel;
using AirIQ.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models;

public partial class Passenger : ObservableObject
{
    public int Id { get; set; }
    public string? Header { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PassportNumber { get; set; }
    public string? Dob { get; set; }
    public string? PassportExpirydate { get; set; }
    public string? PassportIssuingCountryCode { get; set; }
    public string? Nationality { get; set; }
    public PassengerType Type { get; set; }

    [ObservableProperty]
    private bool _isExpanded;

    [ObservableProperty]
    private string? _selectedPassengerType = nameof(PassengerType.Adult);

    [ObservableProperty]
    private string? _selectedTitle = "Mr.";

    [ObservableProperty]
    private string? _travelWith;

    public Infant? AssignedInfant { get; set; }

}
