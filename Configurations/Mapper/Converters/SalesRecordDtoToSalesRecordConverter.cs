using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class SalesRecordDtoToSalesRecordConverter : ConverterBase<SalesRecordDto, SalesRecord>
    {
        protected override SalesRecord ConvertImpl(SalesRecordDto source)
        {
            return new SalesRecord
            {
                Amount = source.Amount.GetValueOrDefault(),
                AirlineName = source.aName,
                EntryDate = source.EntryDate.GetValueOrDefault(),
                FDestName = source.FDestName,
                FinalRate = source.FinalRate.GetValueOrDefault(),
                IsInternationalNew = source.IsInternationalNew.GetValueOrDefault(),
                IsThirdParty = source.IsThirdParty,
                PAX_Qty = source.PAX_Qty.GetValueOrDefault(),
                PNR = source.PNR,
                Prefix = source.Prefix,
                SaleID = source.SaleID.GetValueOrDefault(),
                Status = source.Status.GetValueOrDefault(),
                TravelDateTime = source.TravelDateTime.GetValueOrDefault(),
                Passengers = BackendToAppModelMapper.GetSalesRecordPassengers(source.Passengers ?? Enumerable.Empty<SalesRecordPassengerDto>()).ToList(),
                Infants = BackendToAppModelMapper.GetSalesRecordPassengers(source.Infants ?? Enumerable.Empty<SalesRecordPassengerDto>()).ToList(),
            };
        }
    }
}