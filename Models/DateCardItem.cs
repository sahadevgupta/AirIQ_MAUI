using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.Models
{
    public partial class DateCardItem : ObservableObject
    {
        public DateTime Date { get; }
        public string DayShort => Date.ToString("ddd").ToUpperInvariant();
        public string DateNumber => Date.ToString("dd");
        public string MonthYear => Date.ToString("MMM, yyyy").ToUpperInvariant();
        public string PaxSummary { get; }

        [ObservableProperty]
        private bool _isSelected;

        public DateCardItem(DateTime date, string paxSummary)
        {
            Date = date;
            PaxSummary = paxSummary;
        }
    }
}