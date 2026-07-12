using AirIQ.Enums;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces
{
    public interface ILookupService
    {
        Task<IEnumerable<CityDto>> GetCitiesAsync();
        Task<IEnumerable<CountryDto>> GetCountriesAsync();
        Task<IEnumerable<DistrictDto>> GetDistrictsAsync();
        Task<IEnumerable<MainCityDto>> GetMainCitiesAsync();
        Task<IEnumerable<StateDto>> GetStatesAsync();
        Task<IEnumerable<LookupItemDto>> GetAccountManagersAsync(AccountManagerType type);
    }
}