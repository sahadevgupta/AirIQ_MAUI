namespace AirIQ.Extensions.Navigation
{
    public record ViewRegistration
    {
#nullable disable
        public Type View { get; set; }
        public Type ViewModel { get; set; }
        public string Name { get; set; }
    }
}
