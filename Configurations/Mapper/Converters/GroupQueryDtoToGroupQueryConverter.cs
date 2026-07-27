using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class GroupQueryDtoToGroupQueryConverter : ConverterBase<GroupQueryResponseDto, GroupQuery>
    {
        protected override GroupQuery ConvertImpl(GroupQueryResponseDto source)
        {
            return new GroupQuery
            {
                AdminNotes = source.AdminNotes,
                GroupQueryId = source.GroupQueryID,
                Pax = source.Pax,
                QuotedFare = source.QuotedFare,
                RequestDate = source.RequestDate,
                RouteDetails = source.RouteDetails,
                Status = source.Status,
                TicketType = source.TicketType,
                TravelDates = source.TravelDates
            };
        }
    }
}