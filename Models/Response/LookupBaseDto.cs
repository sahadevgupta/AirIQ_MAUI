using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public abstract record LookupBaseDto
    {
        [JsonPropertyName("Status")]
        public bool? Status { get; set; }

        [JsonPropertyName("Desc")]
        public string? Desc { get; set; }

        [JsonPropertyName("Account")]
        public List<object> Account { get; set; } = [];

        [JsonPropertyName("StateEntry")]
        public List<StateDto>? StateEntry { get; set; }
    }
}