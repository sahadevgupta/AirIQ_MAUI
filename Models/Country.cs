namespace AirIQ.Models
{
    public class Country : LookupBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}