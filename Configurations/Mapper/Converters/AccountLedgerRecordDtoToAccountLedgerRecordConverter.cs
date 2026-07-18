using AirIQ.Models;
using AirIQ.Models.Response;

namespace AirIQ.Configurations.Mapper.Converters
{
    public class AccountLedgerRecordDtoToAccountLedgerRecordConverter : ConverterBase<AccountLedgerRecordDto, AccountLedgerRecord>
    {
        protected override AccountLedgerRecord ConvertImpl(AccountLedgerRecordDto source)
        {
            return new AccountLedgerRecord
            {
                Amount = source.Amount,
                Balance = source.Balance,
                Date = source.Date,
                Destination = source.Destination,
                Particulars = source.Particulars,
                RefNo = source.RefNo,
                TravelDate = source.TravelDate
            };
        }
    }
}