using System.ComponentModel;
using System.Windows.Input;

using AirIQ.Models;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace AirIQ.Controls;

/// <summary>
///     Renders one month of AirIQ.Views.TravelDatesPage's calendar as a single SkiaSharp-drawn
///     canvas instead of composing a MAUI Border/Label/BoxView per day cell.
///
///     Earlier versions of this control pooled ~168 MAUI native views per month (a Grid, a selection
///     pill Border, a today-dot BoxView and a Label per cell) and reused those C# objects across
///     month recycling, and separately made the whole TravelDatesPage a DI singleton so it would
///     never be reconstructed. Neither eliminated a multi-second delay every time the page opened -
///     timestamped logs showed the delay recurred identically even when nothing was reconstructed in
///     C#. The reason: Android tears down and recreates a page's underlying native view handlers
///     every time it's (re)attached to the window, regardless of whether the C# VisualElement graph
///     behind them is reused. Collapsing the whole grid into one canvas reduces the native view count
///     for a month from ~168 to 1, which is what actually removes that recurring cost.
/// </summary>
public partial class CalendarMonthView : ContentView
{
    const int Rows = 6;
    const int Cols = 7;
    const int CellCount = Rows * Cols;
    const float PillRadius = 20f;
    const float TodayDotRadius = 2f;
    const float TodayDotOffset = 16f;

    // Approximates the app's "RobotoSemiBold" (registered as a MAUI font alias, not something
    // SkiaSharp can resolve by that name) with the platform's semi-bold system font.
    static readonly SKTypeface DayTypeface =
        SKTypeface.FromFamilyName(null, SKFontStyleWeight.SemiBold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

    readonly PropertyChangedEventHandler _onDayChanged;
    readonly List<CalendarDay> _subscribedDays = new(CellCount);

    SKColor _onyx;
    SKColor _gray300;
    SKColor _primaryColor;

    CalendarMonth? _month;

    public CalendarMonthView()
    {
        InitializeComponent();
        CacheColors();
        _onDayChanged = (_, __) => canvasView.InvalidateSurface();
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
        foreach (var subscribedDay in _subscribedDays)
            subscribedDay.PropertyChanged -= _onDayChanged;
        _subscribedDays.Clear();

        monthLabel.Text = month?.MonthLabel ?? string.Empty;
        _month = month;

        if (month is not null)
        {
            foreach (var day in month.Days)
            {
                day.PropertyChanged += _onDayChanged;
                _subscribedDays.Add(day);
            }
        }

        canvasView.InvalidateSurface();
    }

    void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        var days = _month?.Days;
        if (days is null)
            return;

        float cellWidth = e.Info.Width / (float)Cols;
        float cellHeight = e.Info.Height / (float)Rows;

        using var pillPaint = new SKPaint { Color = _primaryColor, IsAntialias = true, Style = SKPaintStyle.Fill };
        using var dotPaint = new SKPaint { Color = _primaryColor, IsAntialias = true, Style = SKPaintStyle.Fill };
        using var textPaint = new SKPaint { IsAntialias = true };
        using var font = new SKFont(DayTypeface, cellHeight * 0.34f);
        var fontMetrics = font.Metrics;

        for (int i = 0; i < CellCount && i < days.Count; i++)
        {
            var day = days[i];
            if (!day.IsCurrentMonth)
                continue;

            int row = i / Cols;
            int col = i % Cols;
            float cx = (col + 0.5f) * cellWidth;
            float cy = (row + 0.5f) * cellHeight;

            if (day.IsSelectedEndpoint)
                canvas.DrawCircle(cx, cy, PillRadius, pillPaint);

            textPaint.Color = day.IsSelectedEndpoint ? SKColors.White : day.IsDisabled ? _gray300 : _onyx;

            string text = day.DayNumber.ToString();
            float textY = cy - ((fontMetrics.Ascent + fontMetrics.Descent) / 2f);
            canvas.DrawText(text, cx, textY, SKTextAlign.Center, font, textPaint);

            if (day.IsToday && !day.IsSelectedEndpoint)
                canvas.DrawCircle(cx, cy + TodayDotOffset, TodayDotRadius, dotPaint);
        }
    }

    void OnCanvasTapped(object? sender, TappedEventArgs e)
    {
        var days = _month?.Days;
        if (days is null || canvasView.Width <= 0 || canvasView.Height <= 0)
            return;

        var position = e.GetPosition(canvasView);
        if (position is null)
            return;

        int col = (int)(position.Value.X / (canvasView.Width / Cols));
        int row = (int)(position.Value.Y / (canvasView.Height / Rows));

        if (col < 0 || col >= Cols || row < 0 || row >= Rows)
            return;

        int index = (row * Cols) + col;
        if (index >= days.Count)
            return;

        var day = days[index];
        if (day.IsDisabled || DayTappedCommand?.CanExecute(day) != true)
            return;

        DayTappedCommand.Execute(day);
    }

    void CacheColors()
    {
        _onyx = ToSkColor(ResColor("Onyx"));
        _gray300 = ToSkColor(ResColor("Gray300"));
        _primaryColor = ToSkColor(ResColor("PrimaryColor"));
    }

    static SKColor ToSkColor(Color color) =>
        new((byte)(color.Red * 255), (byte)(color.Green * 255), (byte)(color.Blue * 255), (byte)(color.Alpha * 255));

    static Color ResColor(string key) => (Color)Application.Current!.Resources[key];
}
