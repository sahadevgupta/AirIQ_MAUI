using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class CityDtoToCityConverter : ConverterBase<CityDto, City>
    {
        protected override City ConvertImpl(CityDto source)
        {
            return new City
            {
                ApiVendor = source.ApiVendor,
                ApiVendor1 = source.ApiVendor1,
                Desc = source.Desc,
                FDestination = source.FDestination,
                Id = source.CityId,
                Name = source.CityName,
                Notification = source.Notification,
                PromoCityId = source.PromoCityId.GetValueOrDefault(),
                State = source.State,
                Status = source.Status.GetValueOrDefault(),
                ThirdPartyAccount = source.ThirdPartyAccount,
                TicketOwner = source.TicketOwner,
                Account = source.Account,
                StateEntry = BackendToAppModelMapper
                                .GetStates(source.StateEntry ?? Enumerable.Empty<StateDto>())
                                .ToList()
            };
        }
    }
}