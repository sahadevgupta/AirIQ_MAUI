using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

public partial class SummaryPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Commands ]

    [RelayCommand]
    private async Task Cancel()
    {
        var confirmed = await DialogService.DisplayAlertAsync(AppResource.AirIqDialogTitle, AppResource.CancelBookingConfirmationMessage, AppResource.OK, AppResource.Cancel);
        if (confirmed)
        {
            await ShellNavigationService.NavigateBack();
        }
    }

    #endregion
}
