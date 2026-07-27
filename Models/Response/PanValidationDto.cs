using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AirIQ.Models.Response
{
    public record PanValidationDto
    {
        [JsonPropertyName("pan_number")]
        public string? PanNumber { get; set; }

        [JsonPropertyName("pan_status")]
        public string? PanStatus { get; set; }

        [JsonPropertyName("user_full_name")]
        public string? UserFullName { get; set; }

        [JsonPropertyName("pan_type")]
        public string? PanType { get; set; }

        [JsonPropertyName("name_match_score")]
        public string? NameMatchScore { get; set; }
    }
}