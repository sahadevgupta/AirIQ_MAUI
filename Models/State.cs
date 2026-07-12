namespace AirIQ.Models
{
    public class State : LookupBase
    {
        public string Name { get; set; } = string.Empty;

        public string IsCountry { get; set; } = string.Empty;

        public int CountryId { get; set; }

        public List<object> Account1 { get; set; } = [];

        public List<City> CityEntry { get; set; } = [];

        public List<MainCity> CityEntryMain { get; set; } = [];

        public Country? CountryEntry { get; set; }

        public List<District> District { get; set; } = [];
    }
}