using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirIQ.Models.Request
{
    public class GstLiteRequest
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = "sync";

        [JsonPropertyName("data")]
        public GstDataRequest? GstData { get; set; }

        [JsonPropertyName("task_id")]
        public string TaskId { get; set; } = Guid.NewGuid().ToString();
    }

    public class GstDataRequest
    {
        [JsonPropertyName("business_gstin_number")]
        public string? BusinessGstinNumber { get; set; }

        [JsonPropertyName("contact_info")]
        public bool ContactInfo { get; set; } = true;

        [JsonPropertyName("financial_year")]
        public string FinancialYear { get; set; } = GetCurrentFinancialYear();

        [JsonPropertyName("consent")]
        public string Consent { get; set; } = "Y";

        [JsonPropertyName("consent_text")]
        public string ConsentText { get; set; } =
            "I hereby declare my consent agreement for fetching my information via ZOOP API.";

        private static string GetCurrentFinancialYear()
        {
            DateTime today = DateTime.Now;

            int financialYearStart = (today.Month >= 4) ? today.Year : today.Year - 1;
            int financialYearEnd = financialYearStart + 1;

            return $"{financialYearStart}-{financialYearEnd}";
        }
    }
}