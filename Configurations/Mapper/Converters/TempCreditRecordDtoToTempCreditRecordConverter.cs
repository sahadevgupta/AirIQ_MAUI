using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class TempCreditRecordDtoToTempCreditRecordConverter : ConverterBase<TempCreditRecordDto, TempCreditRecord>
    {
        protected override TempCreditRecord ConvertImpl(TempCreditRecordDto source)
        {
            return new TempCreditRecord
            {
                Amount = source.Amount,
                CreditId = source.CreditID,
                Date = source.Date,
                Name = source.Name,
                TempAmount = source.TempAmount
            };
        }
    }
}