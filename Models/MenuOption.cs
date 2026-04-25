using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models;

public partial class MenuOption : ObservableObject
{
    public string? IconSource { get; set; }
    public string? Title { get; set; }
    public string? Route { get; set; }
    //public Type? TargetType { get; set; }
    public int Index { get; set; }

    [ObservableProperty]
    private bool _isSelected;
}

