using SQLite;

namespace AirIQ.Models;

[Table("ApiCallHistory")]
public class ApiCallHistoryEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string CacheKey { get; set; } = string.Empty;

    [Indexed]
    public DateTime CalledAtUtc { get; set; }
}
