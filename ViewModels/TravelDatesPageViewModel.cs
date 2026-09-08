using System.Collections.ObjectModel;
using System.Collections.Specialized;

using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

/// <summary>
///     Backs <see cref="Views.TravelDatesPage"/> - a departure/return date-range calendar. Deliberately
///     has no knowledge of flights, fares or airports; it only ever hands back the two dates the
///     traveler chose, via <see cref="NavigationParamConstants.SelectedTravelDateResult"/> and
///     <see cref="NavigationParamConstants.SelectedReturnDateResult"/>.
/// </summary>
[QueryProperty(nameof(DepartureDate), NavigationParamConstants.InitialDepartureDate)]
[QueryProperty(nameof(ReturnDate), NavigationParamConstants.InitialReturnDate)]
[QueryProperty(nameof(RequestedStage), NavigationParamConstants.DateSelectionStage)]
[QueryProperty(nameof(AllowedDates), NavigationParamConstants.TravelAllowedDates)]
public partial class TravelDatesPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    const int InitialMonthBatch = 4;
    const int LoadMoreBatch = 3;
    const int MaxMonths = 18;

    static readonly DateTime Today = DateTime.Today;

    readonly HashSet<DateTime> _allowedDateSet = new();

    DateTime _nextMonthToGenerate = new(Today.Year, Today.Month, 1);
    int _generatedMonthCount;
    bool _isInitialized;

    #region [ Properties ]

    [ObservableProperty]
    private ObservableCollection<CalendarMonth> _months = new();

    [ObservableProperty]
    private DateTime? _departureDate;

    [ObservableProperty]
    private DateTime? _returnDate;

    [ObservableProperty]
    private bool _isSelectingDeparture = true;

    [ObservableProperty]
    private bool _isSelectingReturn;

    /// <summary>Which tab the page should open on - set once from navigation, consumed in <see cref="LoadDataWhenNavigatedTo"/>.</summary>
    [ObservableProperty]
    private DateSelectionStage _requestedStage = DateSelectionStage.Departure;

    /// <summary>
    ///     Optional route-specific availability restriction (matches the single-date picker it replaces).
    ///     Only ever constrains the departure leg - the return leg is only bounded by the departure date itself.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<DateTime> _allowedDates = new();

    #endregion

    #region [ Property Changed ]

    partial void OnDepartureDateChanged(DateTime? value) => RecomputeSelectionState();

    partial void OnReturnDateChanged(DateTime? value) => RecomputeSelectionState();

    partial void OnIsSelectingReturnChanged(bool value) => RecomputeSelectionState();

    partial void OnAllowedDatesChanged(ObservableCollection<DateTime>? oldValue, ObservableCollection<DateTime> newValue)
    {
        if (oldValue is INotifyCollectionChanged oldNotifying)
            oldNotifying.CollectionChanged -= OnAllowedDatesCollectionChanged;

        if (newValue is INotifyCollectionChanged newNotifying)
            newNotifying.CollectionChanged += OnAllowedDatesCollectionChanged;

        RebuildAllowedDateSet();
        RecomputeSelectionState();
    }

    void OnAllowedDatesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RebuildAllowedDateSet();
        RecomputeSelectionState();
    }

    void RebuildAllowedDateSet()
    {
        _allowedDateSet.Clear();
        foreach (var date in AllowedDates)
            _allowedDateSet.Add(date.Date);
    }

    #endregion

    #region [ Month Generation ]

    void AppendMonths(int count)
    {
        for (int i = 0; i < count && _generatedMonthCount < MaxMonths; i++)
        {
            Months.Add(BuildMonth(_nextMonthToGenerate));
            _nextMonthToGenerate = _nextMonthToGenerate.AddMonths(1);
            _generatedMonthCount++;
        }

        RecomputeSelectionState();
    }

    // Matches CalendarMonthView's fixed 6-row pooled grid: every month - even a 4- or 5-week one -
    // is padded to exactly 42 cells so each rendered month has the same height. That lets the
    // CollectionView hosting them use ItemSizingStrategy="MeasureFirstItem" instead of measuring
    // every loaded month up front, which is what made the calendar slow to open.
    const int CellsPerMonth = 42;

    static CalendarMonth BuildMonth(DateTime monthStart)
    {
        monthStart = new DateTime(monthStart.Year, monthStart.Month, 1);

        // Week starts Monday, matching the reference layout (M T W T F S S).
        int leading = ((int)monthStart.DayOfWeek + 6) % 7;
        var gridStart = monthStart.AddDays(-leading);

        var days = new List<CalendarDay>(CellsPerMonth);
        for (int i = 0; i < CellsPerMonth; i++)
        {
            var date = gridStart.AddDays(i);
            days.Add(new CalendarDay
            {
                Date = date,
                IsCurrentMonth = date.Month == monthStart.Month && date.Year == monthStart.Year
            });
        }

        return new CalendarMonth(monthStart, days);
    }

    #endregion

    #region [ Selection State ]

    void SetStage(DateSelectionStage stage)
    {
        if (stage == DateSelectionStage.Return && !DepartureDate.HasValue)
            stage = DateSelectionStage.Departure;

        IsSelectingDeparture = stage == DateSelectionStage.Departure;
        IsSelectingReturn = stage == DateSelectionStage.Return;
    }

    // Mutates the already-generated day cells in place (rather than rebuilding Months) so the
    // CollectionView's scroll position never jumps when a selection changes.
    void RecomputeSelectionState()
    {
        var departure = DepartureDate?.Date;
        var returnDate = ReturnDate?.Date;
        bool hasRange = departure.HasValue && returnDate.HasValue && returnDate.Value > departure.Value;
        bool restrictToAllowedDates = _allowedDateSet.Count > 0;

        foreach (var month in Months)
        {
            var days = month.Days;

            for (int rowStart = 0; rowStart < days.Count; rowStart += 7)
            {
                int rowEnd = Math.Min(rowStart + 7, days.Count);
                int firstInRangeIndex = -1;
                int lastInRangeIndex = -1;

                for (int i = rowStart; i < rowEnd; i++)
                {
                    var day = days[i];

                    bool isPast = day.Date < Today;
                    bool isBeforeDeparture = IsSelectingReturn && departure.HasValue && day.Date < departure.Value;
                    bool isUnavailable = !IsSelectingReturn && restrictToAllowedDates && !_allowedDateSet.Contains(day.Date);

                    day.IsDisabled = !day.IsCurrentMonth || isPast || isBeforeDeparture || isUnavailable;
                    day.IsToday = day.IsCurrentMonth && day.Date == Today;
                    day.IsSelectedDeparture = departure.HasValue && day.Date == departure.Value;
                    day.IsSelectedReturn = returnDate.HasValue && day.Date == returnDate.Value;

                    bool isWithinRange = hasRange && day.Date >= departure!.Value && day.Date <= returnDate!.Value;

                    if (isWithinRange)
                    {
                        if (firstInRangeIndex < 0)
                            firstInRangeIndex = i;
                        lastInRangeIndex = i;
                    }
                    else
                    {
                        day.RangeShape = RangeBackgroundShape.None;
                    }
                }

                if (firstInRangeIndex >= 0)
                {
                    for (int i = firstInRangeIndex; i <= lastInRangeIndex; i++)
                    {
                        days[i].RangeShape = firstInRangeIndex == lastInRangeIndex
                            ? RangeBackgroundShape.Isolated
                            : i == firstInRangeIndex
                                ? RangeBackgroundShape.Left
                                : i == lastInRangeIndex
                                    ? RangeBackgroundShape.Right
                                    : RangeBackgroundShape.Middle;
                    }
                }
            }
        }
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void SelectDepartureStage() => SetStage(DateSelectionStage.Departure);

    [RelayCommand]
    private void SelectReturnStage() => SetStage(DateSelectionStage.Return);

    [RelayCommand]
    private void SelectDay(CalendarDay day)
    {
        if (day is null || day.IsDisabled)
            return;

        if (IsSelectingDeparture)
        {
            DepartureDate = day.Date;

            if (ReturnDate.HasValue && ReturnDate.Value.Date <= day.Date)
                ReturnDate = null;

            SetStage(DateSelectionStage.Return);
        }
        else
        {
            ReturnDate = day.Date;
        }
    }

    [RelayCommand]
    private void LoadMoreMonths() => AppendMonths(LoadMoreBatch);

    [RelayCommand]
    private async Task Done()
    {
        if (!DepartureDate.HasValue)
            return;

        var parameters = new Dictionary<string, object>
        {
            { NavigationParamConstants.SelectedTravelDateResult, DepartureDate.Value }
        };

        if (ReturnDate.HasValue)
            parameters[NavigationParamConstants.SelectedReturnDateResult] = ReturnDate.Value;

        await ShellNavigationService.NavigateBack(parameters: parameters);
    }

    [RelayCommand]
    private async Task Close() => await ShellNavigationService.NavigateBack();

    #endregion

    #region [ Overrides ]

    public override Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            RebuildAllowedDateSet();
            AppendMonths(InitialMonthBatch);
            SetStage(RequestedStage);
        }

        return Task.CompletedTask;
    }

    #endregion
}
