namespace AirIQ.Enums
{
    /// <summary>
    ///     Describes how the soft range-highlight background behind a <see cref="Models.CalendarDay"/>
    ///     cell should be rounded, so a multi-day departure/return range reads as one continuous band
    ///     instead of a strip of disconnected pills.
    /// </summary>
    public enum RangeBackgroundShape
    {
        /// <summary>Day is outside the selected range - no background.</summary>
        None,

        /// <summary>First in-range day of its calendar row (or the departure date itself) - rounded on the left only.</summary>
        Left,

        /// <summary>Interior day of the range, both neighbours in the same row are also in range - square.</summary>
        Middle,

        /// <summary>Last in-range day of its calendar row (or the return date itself) - rounded on the right only.</summary>
        Right,

        /// <summary>The only in-range day within its row - rounded on all sides.</summary>
        Isolated
    }
}
