using System.Windows.Input;

using AirIQ.Converter;
using AirIQ.Models;

using Microsoft.Maui.Controls.Shapes;

namespace AirIQ.Controls;

/// <summary>
///     Renders one month of AirIQ.Views.TravelDatesPage's calendar as a fixed 6x7 grid of pooled
///     cells built once in the constructor - mirroring the pooled-cell technique <see cref="FareCalendarView"/>
///     and <see cref="CalendarViewV2"/> already use in this codebase. As the outer CollectionView
///     recycles this view across months, only <see cref="Month"/> changes; each pooled cell's
///     BindingContext is swapped to the new month's corresponding <see cref="CalendarDay"/> and its
///     already-configured bindings do the rest, so a month never has to rebuild its visual tree.
/// </summary>
public partial class CalendarMonthView : ContentView
{
    const int Rows = 6;
    const int Cols = 7;

    static readonly RangeShapeToVisibilityConverter RangeVisibilityConverter = new();
    static readonly RangeShapeToCornerRadiusConverter RangeCornerRadiusConverter = new();

    readonly Grid[] _cellRoots = new Grid[Rows * Cols];

    public CalendarMonthView()
    {
        InitializeComponent();
        BuildGrid();
    }

    #region [ Bindable Properties ]

    public static readonly BindableProperty MonthProperty =
        BindableProperty.Create(nameof(Month), typeof(CalendarMonth), typeof(CalendarMonthView), null,
            propertyChanged: OnMonthChanged);

    public CalendarMonth? Month
    {
        get => (CalendarMonth?)GetValue(MonthProperty);
        set => SetValue(MonthProperty, value);
    }

    public static readonly BindableProperty DayTappedCommandProperty =
        BindableProperty.Create(nameof(DayTappedCommand), typeof(ICommand), typeof(CalendarMonthView));

    public ICommand? DayTappedCommand
    {
        get => (ICommand?)GetValue(DayTappedCommandProperty);
        set => SetValue(DayTappedCommandProperty, value);
    }

    #endregion

    static void OnMonthChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CalendarMonthView)bindable).ApplyMonth((CalendarMonth?)newValue);
    }

    void ApplyMonth(CalendarMonth? month)
    {
        monthLabel.Text = month?.MonthLabel ?? string.Empty;

        var days = month?.Days;
        for (int i = 0; i < _cellRoots.Length; i++)
        {
            _cellRoots[i].BindingContext = days != null && i < days.Count ? days[i] : null;
        }
    }

    void BuildGrid()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                int index = (r * Cols) + c;

                var rangeBackground = new Border
                {
                    StrokeThickness = 0,
                    BackgroundColor = ResColor("Primary0"),
                    Margin = new Thickness(0, 3)
                };
                rangeBackground.SetBinding(Border.IsVisibleProperty, new Binding(nameof(CalendarDay.RangeShape), converter: RangeVisibilityConverter));
                rangeBackground.SetBinding(Border.StrokeShapeProperty, new Binding(nameof(CalendarDay.RangeShape), converter: RangeCornerRadiusConverter));

                var pill = new Border
                {
                    StrokeThickness = 0,
                    WidthRequest = 40,
                    HeightRequest = 40,
                    BackgroundColor = ResColor("PrimaryColor"),
                    StrokeShape = new RoundRectangle { CornerRadius = 20 }
                };
                pill.SetBinding(Border.IsVisibleProperty, nameof(CalendarDay.IsSelectedEndpoint));

                var todayDot = new BoxView
                {
                    WidthRequest = 4,
                    HeightRequest = 4,
                    CornerRadius = 2,
                    Color = ResColor("PrimaryColor"),
                    VerticalOptions = LayoutOptions.End,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 0, 0, 4)
                };
                todayDot.SetBinding(BoxView.IsVisibleProperty, nameof(CalendarDay.IsToday));

                var dayLabel = new Label
                {
                    FontFamily = "RobotoSemiBold",
                    FontSize = 15,
                    TextColor = ResColor("Onyx"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    InputTransparent = true
                };
                dayLabel.SetBinding(Label.TextProperty, nameof(CalendarDay.DayNumber));

                var disabledTrigger = new DataTrigger(typeof(Label))
                {
                    Binding = new Binding(nameof(CalendarDay.IsDisabled)),
                    Value = true
                };
                disabledTrigger.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = ResColor("Gray300") });
                dayLabel.Triggers.Add(disabledTrigger);

                var selectedTrigger = new DataTrigger(typeof(Label))
                {
                    Binding = new Binding(nameof(CalendarDay.IsSelectedEndpoint)),
                    Value = true
                };
                selectedTrigger.Setters.Add(new Setter { Property = Label.TextColorProperty, Value = Colors.White });
                dayLabel.Triggers.Add(selectedTrigger);

                // Every visual for the day is gated on IsCurrentMonth: a leading/trailing filler cell
                // shares its Date with the "real" cell shown on that date's own month page, so without
                // this gate a selected/in-range date would also paint a stray pill on the month it's
                // merely padding out.
                var content = new Grid { HeightRequest = 46, WidthRequest = 44 };
                content.SetBinding(Grid.IsVisibleProperty, nameof(CalendarDay.IsCurrentMonth));
                content.Add(rangeBackground);
                content.Add(pill);
                content.Add(todayDot);
                content.Add(dayLabel);

                var cellRoot = new Grid { HeightRequest = 46, WidthRequest = 44 };
                cellRoot.Add(content);

                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, _) =>
                {
                    if (((Grid)s!).BindingContext is CalendarDay day && DayTappedCommand?.CanExecute(day) == true)
                        DayTappedCommand.Execute(day);
                };
                cellRoot.GestureRecognizers.Add(tap);

                _cellRoots[index] = cellRoot;

                CalendarGrid.Add(cellRoot, c, r);
            }
        }
    }

    static Color ResColor(string key) => (Color)Application.Current!.Resources[key];
}
