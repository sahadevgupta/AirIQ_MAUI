using AirIQ.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models;

public partial class PaymentOptionItem : ObservableObject
{
    public string Title { get; set; } = string.Empty;
    public PaymentOptionType OptionType { get; set; }

    [ObservableProperty]
    private bool _isSelected;
}
