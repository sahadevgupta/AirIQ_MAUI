using System.Globalization;

namespace AirIQ.Extensions
{
    public static class CountryExtensions
    {
        public static string GetFlagEmoji(this string countryName)
        {
            try
            {
                var region = CultureInfo
                    .GetCultures(CultureTypes.SpecificCultures)
                    .Select(c => new RegionInfo(c.Name))
                    .GroupBy(r => r.EnglishName)
                    .Select(g => g.First())
                    .FirstOrDefault(r =>
                        r.EnglishName.Equals(countryName, StringComparison.OrdinalIgnoreCase));

                if (region == null)
                    return string.Empty;

                return region.TwoLetterISORegionName.ToFlagEmoji();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToFlagEmoji(this string countryCode)
        {
            countryCode = countryCode.ToUpperInvariant();

            return string.Concat(countryCode.Select(c =>
                char.ConvertFromUtf32(c + 127397)));
        }
    }
}