namespace AirIQ.Models
{
    public class District : LookupBase
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public List<MainCity> CityEntryMain { get; set; } = [];
    }
}