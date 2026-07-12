using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class CountryDtoToCountryConverter : ConverterBase<CountryDto, Country>
    {
        protected override Country ConvertImpl(CountryDto source)
        {
            return new Country
            {
                Id = source.CountryId,
                Name = source.CountryName,
                Desc = source.Desc,
                Status = source.Status.GetValueOrDefault(),
                Account = source.Account,
                StateEntry = BackendToAppModelMapper
                                .GetStates(source.StateEntry ?? Enumerable.Empty<StateDto>())
                                .ToList()
            };
        }
    }
}