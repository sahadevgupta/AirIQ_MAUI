namespace AirIQ.Models
{
    public abstract class LookupBase
    {
        public bool Status { get; set; }

        public string? Desc { get; set; }

        public List<object> Account { get; set; } = [];

        public List<State>? StateEntry { get; set; }
    }
}