using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record CountryBankDetailsDto
    {
        [JsonPropertyName("TransferAccounts")]
        public List<BankAccountDto>? TransferAccounts { get; set; }

        [JsonPropertyName("CashDepositAccounts")]
        public List<BankAccountDto>? CashDepositAccounts { get; set; }

        [JsonPropertyName("UpiDetails")]
        public UpiDetailsDto? UpiDetails { get; set; }
    }
}