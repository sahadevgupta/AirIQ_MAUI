using System.Text.Json.Serialization;

namespace AirIQ.Models.Response
{
    public record LookupItemDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}