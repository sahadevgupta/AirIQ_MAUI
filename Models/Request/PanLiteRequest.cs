using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirIQ.Models.Request
{
    public class PanLiteRequest
    {
        [JsonPropertyName("mode")]
        public string Mode { get; set; } = "sync";

        [JsonPropertyName("data")]
        public PanDataRequest? PanData { get; set; }

        [JsonPropertyName("task_id")]
        public string TaskId { get; set; } = Guid.NewGuid().ToString();
    }

    public class PanDataRequest
    {
        [JsonPropertyName("customer_pan_number")]
        public string? CustomerPanNumber { get; set; }

        [JsonPropertyName("pan_holder_name")]
        public string? PanHolderName { get; set; }

        [JsonPropertyName("consent")]
        public string Consent { get; set; } = "Y";

        [JsonPropertyName("consent_text")]
        public string ConsentText { get; set; } =
            "I hereby declare my consent agreement for fetching my information via ZOOP API";
    }
}