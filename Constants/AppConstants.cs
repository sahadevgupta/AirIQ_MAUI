namespace AirIQ.Constants
{
    public static class AppConstants
    {
        public const string PanRegex = @"^[A-Z]{5}[0-9]{4}[A-Z]$";
        public const string EmailRegex = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
        public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$";
    }
}