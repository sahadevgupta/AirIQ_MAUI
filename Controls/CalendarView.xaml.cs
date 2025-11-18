using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls.Shapes;
using Mopups.Pages;
using Mopups.Services;

namespace AirIQ.Controls;

public partial class CalendarView : PopupPage
{

    public static readonly BindableProperty AllowedDatesProperty =
            BindableProperty.Create(nameof(AllowedDates), typeof(IList<DateTime>), typeof(CalendarView), null, BindingMode.TwoWay);

    public IList<DateTime> AllowedDates
    {
        get => (IList<DateTime>)GetValue(AllowedDatesProperty);
        set => SetValue(AllowedDatesProperty, value);
    }


    #region OPTIMIZED VIEW

    const int ROWS = 6;
    const int COLS = 7;

    readonly Border[,] _cells = new Border[ROWS, COLS];
    readonly Label[,] _labels = new Label[ROWS, COLS];
    DateTime _displayMonth; // first day of month
    DateTime? _selected;

    public event Action<DateTime> DatePicked;

    #endregion

    public CalendarView()
    {
        InitializeComponent();
    }

    void BuildGridStructure()
    {
        // create pooled cells (lightweight: Frame + Label only)
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
                    FontSize = 14
                };


                var border = new Border
                {
                    StrokeShape = new RoundRectangle { CornerRadius = 20 },
                    Padding = 4,
                    Content = label,
                    HeightRequest = 40,
                    WidthRequest = 40,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                var tap = new TapGestureRecognizer();
                int rr = r, cc = c;
                tap.Tapped += (s, e) => OnCellTapped(rr, cc);
                border.GestureRecognizers.Add(tap);


                _cells[r, c] = border;
                _labels[r, c] = label;


                // add to grid
                CalendarGrid.Add(border, c, r);
                Grid.SetRow(border, r);
                Grid.SetColumn(border, c);
            }
        }
    }

    void OnCellTapped(int row, int col)
    {
        var label = _labels[row, col];
        if (label == null || string.IsNullOrWhiteSpace(label.Text))
            return;


        if (!int.TryParse(label.Text, out var day))
            return;


        var picked = new DateTime(_displayMonth.Year, _displayMonth.Month, day);


        if (!AllowedDates.Contains(picked))
            return; // not allowed


        // update selection
        _selected = picked;
        UpdateMonth(_displayMonth);
        DatePicked?.Invoke(picked);


        // close popup after pick
        Task.Run(async () => await MopupService.Instance.PopAsync(true));
    }


    void UpdateMonth(DateTime month)
    {
        // compute first day of the grid (Sun-based)
        var firstOfMonth = new DateTime(month.Year, month.Month, 1);
        int startOffset = (int)firstOfMonth.DayOfWeek; // 0=Sun
        var gridStart = firstOfMonth.AddDays(-startOffset);


        monthLabel.Text = month.ToString("MMMM yyyy");


        for (int r = 0; r < ROWS; r++)
        {
            for (int c = 0; c < COLS; c++)
            {
                var cellDate = gridStart.AddDays(r * COLS + c);
                var label = _labels[r, c];
                var frame = _cells[r, c];


                label.Text = cellDate.Day.ToString();


                bool isCurrentMonth = cellDate.Month == month.Month && cellDate.Year == month.Year;
                bool isAllowed = AllowedDates.Any() ? AllowedDates.Contains(cellDate.Date) : false;
                bool isSelected = _selected.HasValue && _selected.Value.Date == cellDate.Date;


                // Visual rules (direct set - avoids triggers)
                if (!isCurrentMonth)
                {
                    label.Opacity = 0.45;
                    frame.IsEnabled = false;
                }
                else
                {
                    label.Opacity = 1;
                    frame.IsEnabled = true;
                }


                if (!isAllowed || !isCurrentMonth)
                {
                    // disabled look
                    frame.BackgroundColor = Color.FromArgb("#E0E0E0");
                    label.TextColor = Colors.Gray;
                }
                else if (isSelected)
                {
                    frame.BackgroundColor = Colors.DodgerBlue;
                    label.TextColor = Colors.White;
                }
                else
                {
                    frame.BackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
                    label.TextColor = Colors.White;
                }


                // if allowed but not selected, keep PrimaryColor
                if (isAllowed && !isSelected && isCurrentMonth)
                {
                    // keep PrimaryColor
                }
            }
        }

    }

    private void leftArrow_Clicked(object sender, EventArgs e)
    {
        _displayMonth = _displayMonth.AddMonths(-1);
        UpdateMonth(_displayMonth);
    }

    private void rightArrow_Clicked(object sender, EventArgs e)
    {
        _displayMonth = _displayMonth.AddMonths(1);
        UpdateMonth(_displayMonth);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        BuildGridStructure();
        // default to current month
        _displayMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        UpdateMonth(_displayMonth);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // clear binding context & event handlers to reduce references
        DatePicked = null;
        BindingContext = null;
        GC.Collect();
    }
}

public class DayModel
{
    public DateTime Date { get; set; }
    public int Day { get; set; }
    public string DayofWeek { get; set; }
    public bool IsAllowed { get; set; }
    public bool IsSelected { get; set; }
    public bool IsSelectedMonthDate { get; set; }
}