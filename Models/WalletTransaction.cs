using AirIQ.Enums;

namespace AirIQ.Models
{
    public class WalletTransaction
    {
        public string? Title { get; set; }

        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public double Balance { get; set; }

        public WalletTransactionType Type { get; set; }

        public string IconGlyph { get; set; } = string.Empty;

        public bool IsCredit => Type == WalletTransactionType.Credit;

        public string AmountDisplay => $"{(IsCredit ? "+" : "-")} ₹ {Amount:N2}";

        public Color AmountColor => IsCredit ? Color.FromArgb("#1E8E3E") : Color.FromArgb("#D93025");

        public Color IconBackgroundColor => IsCredit ? Color.FromArgb("#E3F5E8") : Color.FromArgb("#E5F3FE");

        public Color IconColor => IsCredit ? Color.FromArgb("#1E8E3E") : Color.FromArgb("#1076BB");
    }
}
