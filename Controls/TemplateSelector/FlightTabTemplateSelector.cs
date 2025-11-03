using AirIQ.Enums;

namespace AirIQ.Controls.TemplateSelector
{
    class FlightTabTemplateSelector : DataTemplateSelector
    {
        public required DataTemplate OneWayTemplate { get; set; }
        public required DataTemplate RoundTripTemplate { get; set; }
        public required DataTemplate DummyTemplate { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is BookingTypes type)
                return type == BookingTypes.OneWay ? OneWayTemplate : RoundTripTemplate;
            return OneWayTemplate;
        }
    }
}
