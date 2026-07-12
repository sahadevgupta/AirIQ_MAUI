using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public abstract record RecordServiceResponse<T>
    {
        [JsonPropertyName("TotalRecords")]
        public int TotalRecords { get; set; }

        [JsonPropertyName("CurrentPage")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("PageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("TotalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("Data")]
        public T? Data { get; set; }
    }
}