using System;
using AirIQ.Enums;
using AirIQ.Models;

namespace AirIQ.Controls.TemplateSelector;

public class PassengerTemplateSelector : DataTemplateSelector
{
    public required DataTemplate AdultTemplate { get; set; }
    public required DataTemplate InfantTemplate { get; set; }
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if (item is Passenger passenger)
        {
            return passenger.Type == PassengerType.Adult ? AdultTemplate : InfantTemplate;
        }
        return AdultTemplate;
    }
}
