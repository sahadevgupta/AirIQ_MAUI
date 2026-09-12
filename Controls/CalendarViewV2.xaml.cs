using System.Collections.Specialized;
using System.Globalization;
using AirIQ.Resources.Strings;
using Microsoft.Maui.Controls.Shapes;
using Mopups.Pages;
using Mopups.Services;

namespace AirIQ.Controls;

/// <summary>
///     Redesigned month-grid date picker popup. Preserves the public API and
///     API-driven "AllowedDates" behavior of <see cref="CalendarView"/> so it can
///     be used as a drop-in replacement, with an improved UI and a Month/Year
///     dropdown selector.
/// </summary>
public partial class CalendarViewV2 : PopupPage
{
    public static readonly BindableProperty AllowedDatesProperty =
            BindableProperty.Create(nameof(AllowedDates), typeof(IList<DateTime>), typeof(CalendarViewV2), null,
                BindingMode.TwoWay, propertyChanged: OnAllowedDatesChanged);

    public IList<DateTime> AllowedDates
    {
        get => (IList<DateTime>)GetValue(AllowedDatesProperty);
        set => SetValue(AllowedDatesProperty, value);
    }

    #region OPTIMIZED VIEW

    const int ROWS = 6;
    const int COLS = 7;
    const int YearsBack = 1;
    const int YearsForward = 3;

    readonly Border[,] _cells = new Border[ROWS, COLS];
    readonly Label[,] _labels = new Label[ROWS, COLS];
    readonly HashSet<DateTime> _allowedSet = new();

    DateTime _displayMonth; // first day of the displayed month
    DateTime? _selected;
    (int row, int col)? _selectedCell;
    bool _isBuilt;
    bool _suppressPickerEvents;

    public event Action<DateTime> DatePicked;

    #endregion

    public CalendarViewV2()
    {
        InitializeComponent();

        daySunLabel.Text = AppResource.DaySun[..1];
        dayMonLabel.Text = AppResource.DayMon[..1];
        dayTueLabel.Text = AppResource.DayTue[..1];
        dayWedLabel.Text = AppResource.DayWed[..1];
        dayThuLabel.Text = AppResource.DayThu[..1];
        dayFriLabel.Text = AppResource.DayFri[..1];
        daySatLabel.Text = AppResource.DaySat[..1];
    }

    void EnsureGridBuilt()
    {
        if (_isBuilt)
            return;

        _isBuilt = true;

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                var label = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 15,
                    FontFamily = "RobotoRegular"
                };

                var border = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = 20 },
                    Stroke = Colors.Transparent,
                    StrokeThickness = 1.5,
                    Padding = 0,
                    Content = label,
                    HeightRequest = 40,
                    WidthRequest = 40,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.White
                };

                var tap = new TapGestureRecognizer();
                int rr = r, cc = c;
                tap.Tapped += (s, e) => OnCellTapped(rr, cc);
                border.GestureRecognizers.Add(tap);

                _cells[r, c] = border;
                _labels[r, c] = label;

                CalendarGrid.Add(border, c, r);
            }
        }
    }

    static void OnAllowedDatesChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (CalendarViewV2)bindable;

        if (oldValue is INotifyCollectionChanged oldNotifying)
            oldNotifying.CollectionChanged -= control.OnAllowedDatesCollectionChanged;

        if (newValue is INotifyCollectionChanged newNotifying)
            newNotifying.CollectionChanged += control.OnAllowedDatesCollectionChanged;

        control.RebuildAllowedSet();

        if (control._isBuilt)
            control.UpdateMonth(control._displayMonth);
    }

    void OnAllowedDatesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        RebuildAllowedSet();

        if (_isBuilt)
            UpdateMonth(_displayMonth);
    }

    void RebuildAllowedSet()
    {
        _allowedSet.Clear();

        if (AllowedDates == null)
            return;

        foreach (var date in AllowedDates)
            _allowedSet.Add(date.Date);
    }

    void OnCellTapped(int row, int col)
    {
        var label = _labels[row, col];
        if (label == null || string.IsNullOrWhiteSpace(label.Text))
            return;

        if (!int.TryParse(label.Text, out var day))
            return;

        var picked = new DateTime(_displayMonth.Year, _displayMonth.Month, day);

        if (!_allowedSet.Contains(picked))
            return; // not allowed

        var previousCell = _selectedCell;

        _selected = picked;
        _selectedCell = (row, col);

        // only repaint the cells whose visual state actually changed
        if (previousCell.HasValue && previousCell.Value != (row, col))
        {
            var (pr, pc) = previousCell.Value;
            ApplyCellVisual(pr, pc, GridStart(_displayMonth).AddDays(pr * COLS + pc), _displayMonth);
        }

        ApplyCellVisual(row, col, picked, _displayMonth);

        DatePicked?.Invoke(picked);

        // close popup after pick
        Task.Run(async () => await MopupService.Instance.PopAsync(true));
    }

    static DateTime GridStart(DateTime month)
    {
        var firstOfMonth = new DateTime(month.Year, month.Month, 1);
        return firstOfMonth.AddDays(-(int)firstOfMonth.DayOfWeek); // 0 = Sunday
    }

    void UpdateMonth(DateTime month)
    {
        _displayMonth = new DateTime(month.Year, month.Month, 1);
        _selectedCell = null;

        var gridStart = GridStart(_displayMonth);

        monthLabel.Text = _displayMonth.ToString("MMMM yyyy", CultureInfo.CurrentCulture);

        var minNavigableMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        leftArrow.IsVisible = _displayMonth > minNavigableMonth;

        SyncPickerSelection();

        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                var cellDate = gridStart.AddDays(r * COLS + c);
                ApplyCellVisual(r, c, cellDate, _displayMonth);
            }
        }
    }

    void ApplyCellVisual(int row, int col, DateTime cellDate, DateTime month)
    {
        var label = _labels[row, col];
        var cell = _cells[row, col];

        bool isCurrentMonth = cellDate.Month == month.Month && cellDate.Year == month.Year;

        if (!isCurrentMonth)
        {
            label.Text = string.Empty;
            cell.IsEnabled = false;
            cell.BackgroundColor = Colors.White;
            cell.Stroke = Colors.Transparent;
            return;
        }

        label.Text = cellDate.Day.ToString();
        cell.IsEnabled = true; // gating happens in OnCellTapped, not via IsEnabled

        bool isAllowed = _allowedSet.Contains(cellDate.Date);
        bool isSelected = _selected.HasValue && _selected.Value.Date == cellDate.Date;

        if (isSelected)
        {
            _selectedCell = (row, col);
            cell.BackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            cell.Stroke = Colors.Transparent;
            label.TextColor = Colors.White;
            label.FontAttributes = FontAttributes.Bold;
        }
        else if (isAllowed)
        {
            cell.BackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            cell.Stroke = (Color)Application.Current.Resources["PrimaryColor"];
            label.TextColor = Colors.White;
            label.FontAttributes = FontAttributes.Bold;
        }
        else
        {
            cell.BackgroundColor = Colors.White;
            cell.Stroke = Colors.Transparent;
            label.TextColor = (Color)Application.Current.Resources["Gray300"];
            label.FontAttributes = FontAttributes.None;
        }
    }

    void MonthYearTapped(object sender, EventArgs e)
    {
        pickerRow.IsVisible = !pickerRow.IsVisible;

        if (pickerRow.IsVisible)
            SyncPickerSelection();
    }

    void SyncPickerSelection()
    {
        _suppressPickerEvents = true;

        monthPicker.ItemsSource ??= CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
            .Where(m => !string.IsNullOrEmpty(m))
            .ToList();

        yearPicker.ItemsSource ??= Enumerable
            .Range(DateTime.Today.Year - YearsBack, YearsBack + YearsForward + 1)
            .Select(y => y.ToString())
            .ToList();

        monthPicker.SelectedIndex = _displayMonth.Month - 1;
        yearPicker.SelectedItem = _displayMonth.Year.ToString();

        _suppressPickerEvents = false;
    }

    void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_suppressPickerEvents || monthPicker.SelectedIndex < 0)
            return;

        NavigateToMonth(new DateTime(_displayMonth.Year, monthPicker.SelectedIndex + 1, 1));
    }

    void YearPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_suppressPickerEvents || yearPicker.SelectedItem is not string yearText)
            return;

        NavigateToMonth(new DateTime(int.Parse(yearText), _displayMonth.Month, 1));
    }

    void NavigateToMonth(DateTime candidate)
    {
        var minNavigableMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        if (candidate < minNavigableMonth)
            candidate = minNavigableMonth;

        UpdateMonth(candidate);
    }

    private void leftArrow_Clicked(object sender, EventArgs e)
    {
        UpdateMonth(_displayMonth.AddMonths(-1));
    }

    private void rightArrow_Clicked(object sender, EventArgs e)
    {
        UpdateMonth(_displayMonth.AddMonths(1));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        EnsureGridBuilt();
        RebuildAllowedSet();

        _selected = null;
        pickerRow.IsVisible = false;
        UpdateMonth(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        DatePicked = null;

        if (AllowedDates is INotifyCollectionChanged notifying)
            notifying.CollectionChanged -= OnAllowedDatesCollectionChanged;

        BindingContext = null;
    }
}
