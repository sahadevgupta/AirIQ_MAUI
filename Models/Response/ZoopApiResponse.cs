using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirIQ.Models.Response
{
    public record ZoopApiResponse<T>
    {
        [JsonPropertyName("request_id")]
        public string? RequestId { get; set; }

        [JsonPropertyName("task_id")]
        public string? TaskId { get; set; }

        [JsonPropertyName("group_id")]
        public string? GroupId { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("response_code")]
        public string? ResponseCode { get; set; }

        [JsonPropertyName("response_message")]
        public string? ResponseMessage { get; set; }

        [JsonPropertyName("metadata")]
        public ZoopMetadataDto? Metadata { get; set; }

        [JsonPropertyName("result")]
        public T? Result { get; set; }

        [JsonPropertyName("request_timestamp")]
        public DateTime RequestTimestamp { get; set; }

        [JsonPropertyName("response_timestamp")]
        public DateTime ResponseTimestamp { get; set; }
    }

    public class ZoopMetadataDto
    {
        [JsonPropertyName("billable")]
        public string? Billable { get; set; }
    }
}