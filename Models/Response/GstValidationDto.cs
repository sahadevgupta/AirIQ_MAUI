using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirIQ.Models.Response;

public record GstValidationDto
{
    [JsonPropertyName("business_constitution")]
    public string? BusinessConstitution { get; set; }

    [JsonPropertyName("business_nature")]
    public List<string>? BusinessNature { get; set; }

    [JsonPropertyName("central_jurisdiction")]
    public string? CentralJurisdiction { get; set; }

    [JsonPropertyName("central_jurisdiction_code")]
    public string? CentralJurisdictionCode { get; set; }

    [JsonPropertyName("current_registration_status")]
    public string? CurrentRegistrationStatus { get; set; }

    [JsonPropertyName("gstin")]
    public string? Gstin { get; set; }

    [JsonPropertyName("last_updated")]
    public string? LastUpdated { get; set; }

    [JsonPropertyName("legal_name")]
    public string? LegalName { get; set; }

    [JsonPropertyName("other_business_address")]
    public List<OtherBusinessAddressDto>? OtherBusinessAddress { get; set; }

    [JsonPropertyName("primary_business_address")]
    public PrimaryBusinessAddressDto? PrimaryBusinessAddress { get; set; }

    [JsonPropertyName("register_cancellation_date")]
    public string? RegisterCancellationDate { get; set; }

    [JsonPropertyName("register_date")]
    public string? RegisterDate { get; set; }

    [JsonPropertyName("state_jurisdiction")]
    public string? StateJurisdiction { get; set; }

    [JsonPropertyName("state_jurisdiction_code")]
    public string? StateJurisdictionCode { get; set; }

    [JsonPropertyName("tax_payer_type")]
    public string? TaxPayerType { get; set; }

    [JsonPropertyName("trade_name")]
    public string? TradeName { get; set; }
}

public class PrimaryBusinessAddressDto
{
    [JsonPropertyName("building_name")]
    public string? BuildingName { get; set; }

    [JsonPropertyName("building_number")]
    public string? BuildingNumber { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("district")]
    public string? District { get; set; }

    [JsonPropertyName("flat_number")]
    public string? FlatNumber { get; set; }

    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    [JsonPropertyName("business_nature")]
    public string? BusinessNature { get; set; }

    [JsonPropertyName("pincode")]
    public string? Pincode { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("state_code")]
    public string? StateCode { get; set; }

    [JsonPropertyName("full_address")]
    public string? FullAddress { get; set; }
}

public class OtherBusinessAddressDto
{
    [JsonPropertyName("pincode")]
    public string? Pincode { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("district")]
    public string? District { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }

    [JsonPropertyName("building_number")]
    public string? BuildingNumber { get; set; }

    [JsonPropertyName("building_name")]
    public string? BuildingName { get; set; }

    [JsonPropertyName("full_address")]
    public string? FullAddress { get; set; }
}