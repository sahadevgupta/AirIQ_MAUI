using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class MainCityDtoToMainCityConverter : ConverterBase<MainCityDto, MainCity>
    {
        protected override MainCity ConvertImpl(MainCityDto source)
        {
            return new MainCity
            {
                CityEntryMainId = source.CityEntryMainId,
                CityName = source.CityName,
                Desc = source.Desc,
                DistrictId = source.DistrictId,
                State = source.State,
                Status = source.Status.GetValueOrDefault(),
                Account = source.Account,
                StateEntry = BackendToAppModelMapper
                                .GetStates(source.StateEntry ?? Enumerable.Empty<StateDto>())
                                .ToList(),
                District = BackendToAppModelMapper.GetDistrict(source.District)
            };
        }
    }
}