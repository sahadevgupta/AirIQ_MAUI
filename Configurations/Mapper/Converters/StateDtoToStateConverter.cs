using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class StateDtoToStateConverter : ConverterBase<StateDto, State>
    {
        protected override State ConvertImpl(StateDto source)
        {
            return new State
            {
                CountryId = source.CountryId,
                Desc = source.Desc,
                IsCountry = source.IsCountry,
                Name = source.StateName,
                Status = source.Status.GetValueOrDefault(),
                Account = source.Account,
                Account1 = source.Account1,
                CountryEntry = BackendToAppModelMapper.GetCountry(source.CountryEntry),
                StateEntry = BackendToAppModelMapper
                                .GetStates(source.StateEntry ?? Enumerable.Empty<StateDto>())
                                .ToList(),
                CityEntry = BackendToAppModelMapper
                                .GetCities(source.CityEntry ?? Enumerable.Empty<CityDto>())
                                .ToList(),
                CityEntryMain = BackendToAppModelMapper
                                .GetMainCities(source.CityEntryMain ?? Enumerable.Empty<MainCityDto>())
                                .ToList(),
                District = BackendToAppModelMapper
                                .GetDistricts(source.District ?? Enumerable.Empty<DistrictDto>())
                                .ToList()
            };
        }
    }
}