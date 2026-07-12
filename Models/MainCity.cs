namespace AirIQ.Models
{
    public class MainCity : LookupBase
    {
        public int CityEntryMainId { get; set; }

        public string CityName { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public int DistrictId { get; set; }

        public District? District { get; set; }
    }
}