using AirIQ.Configurations.CustomExceptions;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

namespace AirIQ.Services
{
    public class LookupService(IApiServiceBaseParams apiServiceBaseParams) : ApiServiceBase(apiServiceBaseParams), ILookupService
    {
        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
        {
            IEnumerable<CountryDto> countryDtos = Enumerable.Empty<CountryDto>();
            try
            {
                await Connectivity.CheckConnected();

                var apiResponse = await BackendService.GetCountries().ConfigureAwait(false);
                countryDtos = apiResponse ?? Enumerable.Empty<CountryDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return countryDtos;
        }

        public async Task<IEnumerable<CityDto>> GetCitiesAsync()
        {
            IEnumerable<CityDto> cityDtos = Enumerable.Empty<CityDto>();
            try
            {
                await Connectivity.CheckConnected();

                var apiResponse = await BackendService.GetCities().ConfigureAwait(false);
                cityDtos = apiResponse ?? Enumerable.Empty<CityDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return cityDtos;
        }

        public async Task<IEnumerable<StateDto>> GetStatesAsync()
        {
            IEnumerable<StateDto> stateDtos = Enumerable.Empty<StateDto>();
            try
            {
                await Connectivity.CheckConnected();

                var apiResponse = await BackendService.GetStates().ConfigureAwait(false);
                stateDtos = apiResponse ?? Enumerable.Empty<StateDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return stateDtos;
        }

        public async Task<IEnumerable<MainCityDto>> GetMainCitiesAsync()
        {
            IEnumerable<MainCityDto> mainCityDtos = Enumerable.Empty<MainCityDto>();
            try
            {
                await Connectivity.CheckConnected();

                var apiResponse = await BackendService.GetMainCities().ConfigureAwait(false);
                mainCityDtos = apiResponse ?? Enumerable.Empty<MainCityDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return mainCityDtos;
        }

        public async Task<IEnumerable<DistrictDto>> GetDistrictsAsync()
        {
            IEnumerable<DistrictDto> districtDtos = Enumerable.Empty<DistrictDto>();
            try
            {
                await Connectivity.CheckConnected();

                var apiResponse = await BackendService.GetDistricts().ConfigureAwait(false);
                districtDtos = apiResponse ?? Enumerable.Empty<DistrictDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return districtDtos;
        }

        public async Task<IEnumerable<LookupItemDto>> GetAccountManagersAsync(AccountManagerType type)
        {
            IEnumerable<LookupItemDto> lookupItemDtos = Enumerable.Empty<LookupItemDto>();
            try
            {
                await Connectivity.CheckConnected();

                var apiResponse = await BackendService.GetAccountManagers(type).ConfigureAwait(false);
                lookupItemDtos = apiResponse ?? Enumerable.Empty<LookupItemDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return lookupItemDtos;
        }
    }
}