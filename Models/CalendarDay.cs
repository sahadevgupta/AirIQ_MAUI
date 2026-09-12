using AirIQ.Enums;

using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models;

/// <summary>
///     A single tappable date cell inside <see cref="CalendarMonth"/>. Deliberately free of any
///     flight-search/business concepts (fares, holidays, availability) - it only knows how to
///     present itself as part of a departure/return date-range selection.
///
///     Selection/range flags are mutated in place by the owning view model whenever the
///     departure or return date changes, rather than the day list being rebuilt - this keeps the
///     calendar's scroll position stable across selections.
/// </summary>
public partial class CalendarDay : ObservableObject
{
    public DateTime Date { get; init; }

    /// <summary>False for the leading/trailing filler cells that pad a month out to full weeks.</summary>
    public bool IsCurrentMonth { get; init; } = true;

    public int DayNumber => Date.Day;

    [ObservableProperty]
    private bool _isToday;

    /// <summary>True when the day cannot be tapped - past dates, filler cells, or (while picking a return date) dates before the departure date.</summary>
    [ObservableProperty]
    private bool _isDisabled;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSelectedEndpoint))]
    private bool _isSelectedDeparture;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSelectedEndpoint))]
    private bool _isSelectedReturn;

    [ObservableProperty]
    private RangeBackgroundShape _rangeShape = RangeBackgroundShape.None;

    /// <summary>True when this cell is the solid departure or return pill - drives the White text/pill overlay.</summary>
    public bool IsSelectedEndpoint => IsSelectedDeparture || IsSelectedReturn;
}
