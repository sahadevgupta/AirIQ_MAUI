using SQLite;

namespace AirIQ.Models;

[Table("ApiCache")]
public class ApiCacheEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Unique = true)]
    public string CacheKey { get; set; } = string.Empty;

    public string ResponseJson { get; set; } = string.Empty;

    public DateTime LastUpdatedUtc { get; set; }

    public DateTime NextAllowedCallUtc { get; set; }
}
