using AirIQ.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace AirIQ.Model
{
    public partial class TabModel : ObservableObject
    {
        [ObservableProperty]
        string? name;

        [ObservableProperty]
        private bool isSelected;

        public ICommand? TabCommand { get; set; }
        public BookingTypes type { get; set; }

        public string? key { get; set; }

    }
}
