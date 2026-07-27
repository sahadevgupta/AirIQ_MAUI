using System.Collections.ObjectModel;

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
            new MenuOption{Title=AppResource.AccountsLedger, IconSource="manage_accounts", MenuType = MenuType.AccountsLedger},
            new MenuOption{Title=AppResource.UploadRequest, IconSource="upload_file", MenuType = MenuType.UploadRequest},
            new MenuOption{Title=AppResource.TemporaryCredit, IconSource="credit_card", MenuType=MenuType.TemporaryCredit},
            new MenuOption{Title=AppResource.BankDetails, IconSource="account_balance", MenuType = MenuType.BankDetails},
            new MenuOption{Title=AppResource.GroupQuery,IconSource="question_exchange", MenuType = MenuType.GroupQuery},
            new MenuOption{Title=AppResource.PaxCalendar,IconSource="pax_calendar", MenuType=MenuType.PaxCalendar},
            new MenuOption{Title=AppResource.OnlineRecharge,MenuType = MenuType.OnlineRecharge},
        };
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
                await ShellNavigationService.Navigate<SalesRecordPage>();
                break;
            case MenuType.RefundsRecord:
                await ShellNavigationService.Navigate<RefundsRecordPage>();
                break;
            case MenuType.AccountsLedger:
                await ShellNavigationService.Navigate<AccountLedgerRecordPage>();
                break;
            case MenuType.BankDetails:
                await ShellNavigationService.Navigate<BankDetailsPage>();
                break;
            case MenuType.TemporaryCredit:
                await ShellNavigationService.Navigate<TempCreditPage>();
                break;
            case MenuType.UploadRequest:
                await ShellNavigationService.Navigate<UploadRequestPage>();
                break;
            case MenuType.GroupQuery:
                await ShellNavigationService.Navigate<GroupQueryPage>();
                break;
            case MenuType.PaxCalendar:
                await ShellNavigationService.Navigate<PaxCalendarPage>();
                break;
            case MenuType.OnlineRecharge:
                await ShellNavigationService.Navigate<OnlineRechargePage>();
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
