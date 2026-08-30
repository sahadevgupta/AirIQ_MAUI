using System.Collections.ObjectModel;

using AirIQ.Configurations;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.Views;

using AirIQ_MAUI.Views;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Mopups.Services;

namespace AirIQ.ViewModels.Common;

public partial class MenuPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private ObservableCollection<MenuOption> _menus = new ObservableCollection<MenuOption>();

    [ObservableProperty]
    private string _buildNumber = string.Empty;

    #endregion

    #region [ Methods & Service Calls ]

    private void InitializeData()
    {
        PopuplateMenuOptions();
    }

    private void PopuplateMenuOptions()
    {

        BuildNumber = $"App Version: {AppInfo.Current.VersionString}";
        Menus = new ObservableCollection<MenuOption>
        {
            new MenuOption{Title=AppResource.Flights, IconSource="flight", MenuType = MenuType.Flight },
            new MenuOption{Title=AppResource.SalesRecord, IconSource="finance_mode", MenuType= MenuType.SalesRecord},
            new MenuOption{Title=AppResource.RefundsRecord, IconSource="currency_exchange", MenuType = MenuType.RefundsRecord},
            new MenuOption{Title=AppResource.Account, IconSource="account_circle", MenuType = MenuType.Account},
            new MenuOption{Title=AppResource.AccountsLedger, IconSource="manage_accounts", MenuType = MenuType.AccountsLedger},
            new MenuOption{Title=AppResource.UploadRequest, IconSource="upload_file", MenuType = MenuType.UploadRequest},
            new MenuOption{Title=AppResource.TemporaryCredit, IconSource="credit_card", MenuType=MenuType.TemporaryCredit},
            new MenuOption{Title=AppResource.BankDetails, IconSource="account_balance", MenuType = MenuType.BankDetails},
            new MenuOption{Title=AppResource.GroupQuery,IconSource="question_exchange", MenuType = MenuType.GroupQuery},
            new MenuOption{Title=AppResource.PaxCalendar,IconSource="pax_calendar", MenuType=MenuType.PaxCalendar},
            new MenuOption{Title=AppResource.OnlineRecharge, IconSource="online_recharge", MenuType = MenuType.OnlineRecharge},
        };

        foreach (var menu in Menus)
        {
            menu.IsSelected = menu.MenuType == AppConfiguration.SelectedMenuType;
        }
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void ClosePopup()
    {
        MopupService.Instance.PopAsync();
    }

    [RelayCommand]
    private async Task Logout()
    {
        ClosePopup();
        SecureStorage.RemoveAll();
        Preferences.Clear();

        if (Shell.Current != null)
            await MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//LoginPage"));
    }

    [RelayCommand]
    private async Task Menu(MenuOption selectedMenu)
    {
        ClosePopup();
        switch (selectedMenu.MenuType)
        {
            case MenuType.SalesRecord:
                await ShellNavigationService.NavigateToFlyoutPage<SalesRecordPage>();
                break;
            case MenuType.RefundsRecord:
                await ShellNavigationService.NavigateToFlyoutPage<RefundsRecordPage>();
                break;
            case MenuType.Account:
                await ShellNavigationService.NavigateToFlyoutPage<MyAccountPage>();
                break;
            case MenuType.AccountsLedger:
                await ShellNavigationService.NavigateToFlyoutPage<AccountLedgerRecordPage>();
                break;
            case MenuType.BankDetails:
                await ShellNavigationService.NavigateToFlyoutPage<BankDetailsPage>();
                break;
            case MenuType.TemporaryCredit:
                await ShellNavigationService.NavigateToFlyoutPage<TempCreditPage>();
                break;
            case MenuType.UploadRequest:
                await ShellNavigationService.NavigateToFlyoutPage<UploadRequestPage>();
                break;
            case MenuType.GroupQuery:
                await ShellNavigationService.NavigateToFlyoutPage<GroupQueryPage>();
                break;
            case MenuType.PaxCalendar:
                await ShellNavigationService.NavigateToFlyoutPage<PaxCalendarPage>();
                break;
            case MenuType.OnlineRecharge:
                await ShellNavigationService.NavigateToFlyoutPage<OnlineRechargePage>();
                break;
            case MenuType.Flight:
                await Shell.Current.GoToAsync("///home");
                break;
        }
    }

    #endregion

    #region [ override Methods ]

    public override Task LoadDataWhenNavigatedTo()
    {
        InitializeData();
        return base.LoadDataWhenNavigatedTo();
    }



    #endregion

}
