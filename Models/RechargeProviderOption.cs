using AirIQ.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models;

public partial class RechargeProviderOption : ObservableObject
{
    public string Title { get; set; } = string.Empty;
    public RechargeProviderType ProviderType { get; set; }

    [ObservableProperty]
    private bool _isSelected;
}
