using System;
using AirIQ.Enums;

namespace AirIQ.Models;

public class Passenger
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PassportNumber { get; set; }
    public string? Dob { get; set; }
    public string? PassportExpirydate { get; set; }
    public string? PassportIssuingCountryCode { get; set; }
    public string? Nationality { get; set; }
    public PassengerType PassengerType { get; set; }

}
