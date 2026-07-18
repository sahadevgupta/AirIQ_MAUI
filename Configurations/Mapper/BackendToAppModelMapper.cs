using AirIQ.Configurations.Mapper.Converters;
using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper;

public static class BackendToAppModelMapper
{
    public static User GetUser(UserDto? userDto)
    {
        if (userDto == null)
        {
            return new User();
        }
        var converter = new UserDtoToUseConverter();
        return converter.Convert(userDto);
    }

    public static IEnumerable<FlightRoute> GetAvailableRoutes(IEnumerable<FlightRouteDto> flightRouteDtos)
    {
        if (flightRouteDtos == null)
        {
            return new List<FlightRoute>();
        }
        var converter = new FlightRouteDtoToFlightRouteConverter();
        return flightRouteDtos.Select(converter.Convert);
    }

    public static IEnumerable<Flight> GetFlights(IEnumerable<FlightSearchResultDto> flightSearchResultDtos)
    {
        if (flightSearchResultDtos == null)
        {
            return new List<Flight>();
        }
        var converter = new FlightSearchResultDtoToFlight();
        return flightSearchResultDtos.Select(converter.Convert);
    }

    public static IEnumerable<Country> GetCountries(IEnumerable<CountryDto> countryDtos)
    {
        if (countryDtos == null)
        {
            return new List<Country>();
        }
        var converter = new CountryDtoToCountryConverter();
        return countryDtos.Select(converter.Convert);
    }

    public static IEnumerable<State> GetStates(IEnumerable<StateDto> stateDtos)
    {
        if (stateDtos == null)
        {
            return new List<State>();
        }
        var converter = new StateDtoToStateConverter();
        return stateDtos.Select(converter.Convert);
    }

    public static IEnumerable<City> GetCities(IEnumerable<CityDto> cityDtos)
    {
        if (cityDtos == null)
        {
            return new List<City>();
        }
        var converter = new CityDtoToCityConverter();
        return cityDtos.Select(converter.Convert);
    }

    public static IEnumerable<MainCity> GetMainCities(IEnumerable<MainCityDto> mainCityDtos)
    {
        if (mainCityDtos == null)
        {
            return new List<MainCity>();
        }
        var converter = new MainCityDtoToMainCityConverter();
        return mainCityDtos.Select(converter.Convert);
    }

    public static IEnumerable<District> GetDistricts(IEnumerable<DistrictDto> districtDtos)
    {
        if (districtDtos == null)
        {
            return new List<District>();
        }
        var converter = new DistrictDtoToDisctrictConverter();
        return districtDtos.Select(converter.Convert);
    }

    public static IEnumerable<LookupItem> GetLookupItems(IEnumerable<LookupItemDto> lookupItemDtos)
    {
        if (lookupItemDtos == null)
        {
            return new List<LookupItem>();
        }
        var converter = new LookupItemDtoToLookupItemConverter();
        return lookupItemDtos.Select(converter.Convert);
    }

    public static IEnumerable<SalesRecord> GetSalesRecords(IEnumerable<SalesRecordDto> salesRecordDtos)
    {
        if (salesRecordDtos == null)
        {
            return new List<SalesRecord>();
        }
        var converter = new SalesRecordDtoToSalesRecordConverter();
        return salesRecordDtos.Select(converter.Convert);
    }

    public static IEnumerable<RefundRecord> GetRefundRecords(IEnumerable<RefundRecordDto> refundRecordDtos)
    {
        if (refundRecordDtos == null)
        {
            return new List<RefundRecord>();
        }
        var converter = new RefundRecordDtoToRefundRecordConverter();
        return refundRecordDtos.Select(converter.Convert);
    }

    public static IEnumerable<AccountLedgerRecord> GetAccountLedgerRecords(IEnumerable<AccountLedgerRecordDto> accountLedgerRecordDtos)
    {
        if (accountLedgerRecordDtos == null)
        {
            return new List<AccountLedgerRecord>();
        }
        var converter = new AccountLedgerRecordDtoToAccountLedgerRecordConverter();
        return accountLedgerRecordDtos.Select(converter.Convert);
    }

    public static IEnumerable<SalesRecordPassenger> GetSalesRecordPassengers(IEnumerable<SalesRecordPassengerDto> salesRecordPassengerDtos)
    {
        if (salesRecordPassengerDtos == null)
        {
            return new List<SalesRecordPassenger>();
        }
        var converter = new SalesRecordPassengerDtoToSalesRecordPassengerConverter();
        return salesRecordPassengerDtos.Select(converter.Convert);
    }

    public static District GetDistrict(DistrictDto? districtDto)
    {
        if (districtDto == null)
        {
            return new District();
        }
        var converter = new DistrictDtoToDisctrictConverter();
        return converter.Convert(districtDto);
    }

    public static Country GetCountry(CountryDto? countryDto)
    {
        if (countryDto == null)
        {
            return new Country();
        }
        var converter = new CountryDtoToCountryConverter();
        return converter.Convert(countryDto);
    }
}
