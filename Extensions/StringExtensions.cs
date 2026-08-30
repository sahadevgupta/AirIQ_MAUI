namespace AirIQ.Extensions
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string? source, string? value)
            => !string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(value) && source.Contains(value, StringComparison.OrdinalIgnoreCase);
    }
}
