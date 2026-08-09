using AirIQ.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace AirIQ.Models
{
    public partial class TabModel : ObservableObject
    {
        [ObservableProperty]
        string? name;

        [ObservableProperty]
        private bool isSelected;

        public ICommand? TabCommand { get; set; }

        public string? key { get; set; }
        public string? Icon { get; set; }
    }
}
