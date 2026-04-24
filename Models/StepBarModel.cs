using System;
using AirIQ.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models;

public partial class StepBarModel : ObservableObject
{
    public int StepID { get; set; }
    public bool IsNotLast { get; set; }
    public bool IsFirst { get; set; }
    public double ListWidth { get; set; }

    [ObservableProperty]
    private string? _stepName;

    [ObservableProperty]
    private StepBarStatus _status;

    [ObservableProperty]
    private double _progressValue;

    [ObservableProperty]
    private ContentView? _mainContent;

    [ObservableProperty]
    private bool _isCurrentContent;
}
