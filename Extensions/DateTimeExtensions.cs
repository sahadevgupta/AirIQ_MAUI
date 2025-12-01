using System;

namespace AirIQ.Extensions;

public static class DateTimeExtensions
{
    public static DateTime ToDate(this int epoch)
    {
        var epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var date = epochStart.AddSeconds(epoch);
        return date;
    }

}
