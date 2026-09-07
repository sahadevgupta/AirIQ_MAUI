namespace AirIQ.Controls.Models;

/// <summary>
///     Describes the presentation for a single date inside <see cref="FareCalendarView"/>.
///     Purely a display model - any business rules (which dates are holidays, which
///     carry a fare, which are disabled) are decided by the consumer and supplied here.
/// </summary>
public class CalendarDayInfo
{
    public DateTime Date { get; set; }

    /// <summary>Small text shown under the day number (e.g. a fare amount).</summary>
    public string Subtitle { get; set; }

    /// <summary>Short label shown above the day number (e.g. a holiday name).</summary>
    public string Tag { get; set; }

    /// <summary>Marks the date as a holiday - counted in the month's holiday badge and highlighted.</summary>
    public bool IsHoliday { get; set; }

    /// <summary>Generic highlight (e.g. a weekend) independent of <see cref="IsHoliday"/>.</summary>
    public bool IsHighlighted { get; set; }

    /// <summary>Marks the date as unavailable for selection.</summary>
    public bool IsDisabled { get; set; }
}
