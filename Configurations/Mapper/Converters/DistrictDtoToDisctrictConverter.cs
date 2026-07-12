using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class DistrictDtoToDisctrictConverter : ConverterBase<DistrictDto, District>
    {
        protected override District ConvertImpl(DistrictDto source)
        {
            return new District
            {
                Desc = source.Desc,
                Id = source.DistrictId,
                Name = source.Name,
                State = source.State,
                Status = source.Status.GetValueOrDefault(),
                Account = source.Account,
                CityEntryMain = BackendToAppModelMapper
                                .GetMainCities(source.CityEntryMain ?? Enumerable.Empty<MainCityDto>())
                                .ToList(),
                StateEntry = BackendToAppModelMapper
                                .GetStates(source.StateEntry ?? Enumerable.Empty<StateDto>())
                                .ToList()
            };
        }
    }
}