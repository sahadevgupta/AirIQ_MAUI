using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class SalesRecordPassengerDtoToSalesRecordPassengerConverter : ConverterBase<SalesRecordPassengerDto, SalesRecordPassenger>
    {
        protected override SalesRecordPassenger ConvertImpl(SalesRecordPassengerDto source)
        {
            return new SalesRecordPassenger
            {
                InfantCharges = source.InfantCharges,
                PassengerName = source.PassengerName
            };
        }
    }
}