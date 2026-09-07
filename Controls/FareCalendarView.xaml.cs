using System.Collections.Specialized;
using System.Globalization;

using AirIQ.Controls.Models;
using AirIQ.Resources.Strings;

using Microsoft.Maui.Controls.Shapes;

namespace AirIQ.Controls;

/// <summary>
///     Standalone monthly fare-calendar control (month grid with a per-day subtitle,
///     holiday badge and Today indicator). Independent of <see cref="CalendarView"/> and
///     <see cref="CalendarViewV2"/> - does not inherit from or modify either.
/// </summary>
public partial class FareCalendarView : ContentView
{
    const int ROWS = 6;
    const int COLS = 7;

    readonly Border[,] _cells = new Border[ROWS, COLS];
    readonly Label[,] _tagLabels = new Label[ROWS, COLS];
    readonly Label[,] _dayLabels = new Label[ROWS, COLS];
    readonly Label[,] _subtitleLabels = new Label[ROWS, COLS];
    readonly DateTime[,] _cellDates = new DateTime[ROWS, COLS];
    readonly Dictionary<DateTime, CalendarDayInfo> _dayInfoMap = new();

    DateTime _displayMonth;
    bool _isBuilt;
    bool _suppressDisplayedMonthCallback;

    public event EventHandler<DateTime> DateSelected;

    public FareCalendarView()
    {
        InitializeComponent();

        EnsureGridBuilt();
        BuildWeekdayHeader();
        UpdateMonth(DisplayedMonth);
    }

    #region [ Bindable Properties ]

    public static readonly BindableProperty SelectedDateProperty =
        BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(FareCalendarView), null,
            BindingMode.TwoWay, propertyChanged: OnSelectedDateChanged);

    public DateTime? SelectedDate
    {
        get => (DateTime?)GetValue(SelectedDateProperty);
        set => SetValue(SelectedDateProperty, value);
    }

    public static readonly BindableProperty MinimumDateProperty =
        BindableProperty.Create(nameof(MinimumDate), typeof(DateTime), typeof(FareCalendarView),
            defaultValueCreator: _ => DateTime.Today.Date, propertyChanged: OnBoundsChanged);

    public DateTime MinimumDate
    {
        get => (DateTime)GetValue(MinimumDateProperty);
        set => SetValue(MinimumDateProperty, value);
    }

    public static readonly BindableProperty MaximumDateProperty =
        BindableProperty.Create(nameof(MaximumDate), typeof(DateTime), typeof(FareCalendarView),
            defaultValueCreator: _ => DateTime.Today.Date.AddMonths(12), propertyChanged: OnBoundsChanged);

    public DateTime MaximumDate
    {
        get => (DateTime)GetValue(MaximumDateProperty);
        set => SetValue(MaximumDateProperty, value);
    }

    public static readonly BindableProperty DayInfosProperty =
        BindableProperty.Create(nameof(DayInfos), typeof(IEnumerable<CalendarDayInfo>), typeof(FareCalendarView), null,
            propertyChanged: OnDayInfosChanged);

    public IEnumerable<CalendarDayInfo> DayInfos
    {
        get => (IEnumerable<CalendarDayInfo>)GetValue(DayInfosProperty);
        set => SetValue(DayInfosProperty, value);
    }

    public static readonly BindableProperty FirstDayOfWeekProperty =
        BindableProperty.Create(nameof(FirstDayOfWeek), typeof(DayOfWeek), typeof(FareCalendarView), DayOfWeek.Monday,
            propertyChanged: OnFirstDayOfWeekChanged);

    public DayOfWeek FirstDayOfWeek
    {
        get => (DayOfWeek)GetValue(FirstDayOfWeekProperty);
        set => SetValue(FirstDayOfWeekProperty, value);
    }

    public static readonly BindableProperty DisplayedMonthProperty =
        BindableProperty.Create(nameof(DisplayedMonth), typeof(DateTime), typeof(FareCalendarView),
            defaultValueCreator: _ => new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
            defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnDisplayedMonthChanged);

    public DateTime DisplayedMonth
    {
        get => (DateTime)GetValue(DisplayedMonthProperty);
        set => SetValue(DisplayedMonthProperty, value);
    }

    #endregion

    #region [ Property Changed Callbacks ]

    static void OnSelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FareCalendarView)bindable;
        if (!control._isBuilt)
            return;

        var targetMonth = newValue is DateTime selected
            ? new DateTime(selected.Year, selected.Month, 1)
            : control._displayMonth;

        control.UpdateMonth(targetMonth);
    }

    static void OnBoundsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FareCalendarView)bindable;
        if (control._isBuilt)
            control.UpdateMonth(control._displayMonth);
    }

    static void OnDayInfosChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FareCalendarView)bindable;

        if (oldValue is INotifyCollectionChanged oldNotifying)
            oldNotifying.CollectionChanged -= control.OnDayInfosCollectionChanged;

        if (newValue is INotifyCollectionChanged newNotifying)
            newNotifying.CollectionChanged += control.OnDayInfosCollectionChanged;

        control.RebuildDayInfoMap();

        if (control._isBuilt)
            control.UpdateMonth(control._displayMonth);
    }

    static void OnFirstDayOfWeekChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FareCalendarView)bindable;
        if (!control._isBuilt)
            return;

        control.BuildWeekdayHeader();
        control.UpdateMonth(control._displayMonth);
    }

    static void OnDisplayedMonthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (FareCalendarView)bindable;
        if (control._suppressDisplayedMonthCallback || !control._isBuilt)
            return;

        var month = new DateTime(((DateTime)newValue).Year, ((DateTime)newValue).Month, 1);
        if (month != control._displayMonth)
            control.UpdateMonth(month);
    }

    void OnDayInfosCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        RebuildDayInfoMap();

        if (_isBuilt)
            UpdateMonth(_displayMonth);
    }

    #endregion

    #region [ Grid Construction ]

    void EnsureGridBuilt()
    {
        if (_isBuilt)
            return;

        _isBuilt = true;

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                var tagLabel = new Label
                {
                    FontSize = 10,
                    FontFamily = "RobotoMedium",
                    HorizontalTextAlignment = TextAlignment.Center,
                    LineBreakMode = LineBreakMode.TailTruncation,
                    MaxLines = 1
                };

                var dayLabel = new Label
                {
                    FontSize = 15,
                    FontFamily = "RobotoSemiBold",
                    HorizontalTextAlignment = TextAlignment.Center
                };

                var subtitleLabel = new Label
                {
                    FontSize = 11,
                    FontFamily = "RobotoRegular",
                    HorizontalTextAlignment = TextAlignment.Center
                };

                var content = new Grid
                {
                    RowDefinitions =
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(GridLength.Auto)
                    },
                    RowSpacing = 2
                };
                content.Add(tagLabel, 0, 0);
                content.Add(dayLabel, 0, 1);
                content.Add(subtitleLabel, 0, 2);

                var cell = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    StrokeThickness = 0,
                    BackgroundColor = Colors.Transparent,
                    Padding = new Thickness(2, 6),
                    Content = content,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                };

                var tap = new TapGestureRecognizer();
                int rr = r, cc = c;
                tap.Tapped += (s, e) => OnCellTapped(rr, cc);
                cell.GestureRecognizers.Add(tap);

                _cells[r, c] = cell;
                _tagLabels[r, c] = tagLabel;
                _dayLabels[r, c] = dayLabel;
                _subtitleLabels[r, c] = subtitleLabel;

                CalendarGrid.Add(cell, c, r);
            }
        }
    }

    void BuildWeekdayHeader()
    {
        WeekdayHeaderGrid.Children.Clear();

        for (int i = 0; i < COLS; i++)
        {
            var day = (DayOfWeek)(((int)FirstDayOfWeek + i) % 7);

            var label = new Label
            {
                Text = DayAbbreviation(day).ToUpper(CultureInfo.CurrentCulture),
                FontSize = 12,
                FontFamily = "RobotoMedium",
                TextColor = ResColor("Gray500"),
                HorizontalTextAlignment = TextAlignment.Center
            };

            WeekdayHeaderGrid.Add(label, i, 0);
        }
    }

    static string DayAbbreviation(DayOfWeek day) => day switch
    {
        DayOfWeek.Sunday => AppResource.DaySun,
        DayOfWeek.Monday => AppResource.DayMon,
        DayOfWeek.Tuesday => AppResource.DayTue,
        DayOfWeek.Wednesday => AppResource.DayWed,
        DayOfWeek.Thursday => AppResource.DayThu,
        DayOfWeek.Friday => AppResource.DayFri,
        DayOfWeek.Saturday => AppResource.DaySat,
        _ => string.Empty
    };

    #endregion

    #region [ Rendering ]

    void RebuildDayInfoMap()
    {
        _dayInfoMap.Clear();

        if (DayInfos == null)
            return;

        foreach (var info in DayInfos)
        {
            if (info != null)
                _dayInfoMap[info.Date.Date] = info;
        }
    }

    static DateTime GridStart(DateTime month, DayOfWeek firstDayOfWeek)
    {
        var firstOfMonth = new DateTime(month.Year, month.Month, 1);
        int diff = ((int)firstOfMonth.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
        return firstOfMonth.AddDays(-diff);
    }

    void UpdateMonth(DateTime month)
    {
        _displayMonth = new DateTime(month.Year, month.Month, 1);

        monthLabel.Text = _displayMonth.ToString("MMM yyyy", CultureInfo.CurrentCulture);
        SetDisplayedMonthValue(_displayMonth);

        var gridStart = GridStart(_displayMonth, FirstDayOfWeek);
        int holidayCount = 0;

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                var cellDate = gridStart.AddDays(r * COLS + c);
                _cellDates[r, c] = cellDate;

                bool isCurrentMonth = cellDate.Month == _displayMonth.Month && cellDate.Year == _displayMonth.Year;

                if (isCurrentMonth && _dayInfoMap.TryGetValue(cellDate.Date, out var info) && info.IsHoliday)
                    holidayCount++;

                ApplyCellVisual(r, c, cellDate, isCurrentMonth);
            }
        }

        holidayBadge.IsVisible = holidayCount > 0;
        if (holidayCount > 0)
            holidayBadgeLabel.Text = string.Format(AppResource.HolidaysCountFormat, holidayCount);

        var minNavigableMonth = new DateTime(MinimumDate.Year, MinimumDate.Month, 1);
        var maxNavigableMonth = new DateTime(MaximumDate.Year, MaximumDate.Month, 1);
        leftArrow.IsVisible = _displayMonth > minNavigableMonth;
        rightArrow.IsVisible = _displayMonth < maxNavigableMonth;
    }

    void ApplyCellVisual(int row, int col, DateTime cellDate, bool isCurrentMonth)
    {
        var cell = _cells[row, col];
        var tagLabel = _tagLabels[row, col];
        var dayLabel = _dayLabels[row, col];
        var subtitleLabel = _subtitleLabels[row, col];

        if (!isCurrentMonth)
        {
            cell.Opacity = 0;
            cell.InputTransparent = true;
            tagLabel.Text = string.Empty;
            dayLabel.Text = string.Empty;
            subtitleLabel.Text = string.Empty;
            return;
        }

        cell.Opacity = 1;

        _dayInfoMap.TryGetValue(cellDate.Date, out var info);

        bool isToday = cellDate.Date == DateTime.Today;
        bool isSelected = SelectedDate.HasValue && SelectedDate.Value.Date == cellDate.Date;
        bool isDisabled = cellDate.Date < MinimumDate.Date || cellDate.Date > MaximumDate.Date || (info?.IsDisabled ?? false);
        bool isHighlighted = info != null && (info.IsHoliday || info.IsHighlighted);

        cell.InputTransparent = isDisabled;

        dayLabel.Text = cellDate.Day.ToString();
        subtitleLabel.Text = info?.Subtitle ?? string.Empty;
        tagLabel.Text = isToday ? AppResource.Today : info?.Tag ?? string.Empty;
        tagLabel.TextColor = isToday ? ResColor("PrimaryColor") : ResColor("CalendarHighlightText");

        if (isSelected)
        {
            cell.BackgroundColor = ResColor("PrimaryColor");
            dayLabel.TextColor = Colors.White;
            subtitleLabel.TextColor = Colors.White;
        }
        else if (isDisabled)
        {
            cell.BackgroundColor = Colors.Transparent;
            dayLabel.TextColor = ResColor("Gray300");
            subtitleLabel.TextColor = ResColor("Gray300");
        }
        else if (isHighlighted)
        {
            cell.BackgroundColor = ResColor("CalendarHighlightBackground");
            dayLabel.TextColor = ResColor("Gray900");
            subtitleLabel.TextColor = ResColor("Gray500");
        }
        else
        {
            cell.BackgroundColor = Colors.Transparent;
            dayLabel.TextColor = ResColor("Gray900");
            subtitleLabel.TextColor = ResColor("Gray500");
        }
    }

    static Color ResColor(string key) => (Color)Application.Current.Resources[key];

    void SetDisplayedMonthValue(DateTime month)
    {
        _suppressDisplayedMonthCallback = true;
        DisplayedMonth = month;
        _suppressDisplayedMonthCallback = false;
    }

    #endregion

    #region [ Interaction ]

    void OnCellTapped(int row, int col)
    {
        var cell = _cells[row, col];
        if (cell.InputTransparent)
            return;

        var picked = _cellDates[row, col].Date;

        SelectedDate = picked;
        DateSelected?.Invoke(this, picked);
    }

    void LeftArrow_Clicked(object sender, EventArgs e) => NavigateToMonth(_displayMonth.AddMonths(-1));

    void RightArrow_Clicked(object sender, EventArgs e) => NavigateToMonth(_displayMonth.AddMonths(1));

    void NavigateToMonth(DateTime candidate)
    {
        var month = new DateTime(candidate.Year, candidate.Month, 1);
        var min = new DateTime(MinimumDate.Year, MinimumDate.Month, 1);
        var max = new DateTime(MaximumDate.Year, MaximumDate.Month, 1);

        if (month < min)
            month = min;
        if (month > max)
            month = max;

        UpdateMonth(month);
    }

    #endregion
}
