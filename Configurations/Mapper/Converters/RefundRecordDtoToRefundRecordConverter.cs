using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class RefundRecordDtoToRefundRecordConverter : ConverterBase<RefundRecordDto, RefundRecord>
    {
        protected override RefundRecord ConvertImpl(RefundRecordDto source)
        {
            return new RefundRecord
            {
                Amount = source.Amount,
                CancelChrg = source.CancelChrg,
                EntryDate = source.EntryDate,
                FDestName = source.FDestName,
                IsThirdParty = source.IsThirdParty,
                PNR = source.PNR,
                Prefix = source.Prefix,
                Qty = source.Qty,
                RefundAmount = source.RefundAmount,
                ReturnId = source.ReturnID,
                ThirdReturnId = source.ThirdReturnID,
                TravelDateTime = source.TravelDateTime
            };
        }
    }
}