using System.Globalization;

namespace AirIQ.Models;

/// <summary>
///     One page of the vertically-scrolling calendar: a month label plus every day cell needed to
///     render its grid (including leading/trailing filler days so the list length is a multiple of 7).
/// </summary>
public class CalendarMonth
{
    public CalendarMonth(DateTime monthStart, IReadOnlyList<CalendarDay> days)
    {
        MonthStart = monthStart;
        MonthLabel = monthStart.ToString("MMMM yyyy", CultureInfo.CurrentCulture);
        Days = days;
    }

    public DateTime MonthStart { get; }

    public string MonthLabel { get; }

    public IReadOnlyList<CalendarDay> Days { get; }
}
