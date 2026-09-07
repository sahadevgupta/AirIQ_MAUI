using System.Collections.ObjectModel;

using AirIQ.Constants;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    [QueryProperty(nameof(AllowedDates), NavigationParamConstants.TravelAllowedDates)]
    public partial class DepartureDatePageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        [ObservableProperty]
        private ObservableCollection<DateTime> _allowedDates = new();

        [ObservableProperty]
        private DateTime? _selectedDate;

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task ConfirmSelection(DateTime date)
        {
            await ShellNavigationService.NavigateBack(parameters: new Dictionary<string, object>
            {
                { NavigationParamConstants.SelectedTravelDateResult, date }
            });
        }

        #endregion
    }
}
