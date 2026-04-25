using System;
using System.Collections.ObjectModel;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace AirIQ.ViewModels.Common;

public partial class MenuPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private ObservableCollection<MenuOption> _menus = new ObservableCollection<MenuOption>();
    #endregion

    #region [ Methods & Service Calls ]

    private void InitializeData()
    {
        PopuplateMenuOptions();
    }

    private void PopuplateMenuOptions()
    {
        Menus = new ObservableCollection<MenuOption>
        {
            new MenuOption{Title=AppResource.Flights, IconSource="flight"},
            new MenuOption{Title=AppResource.SalesRecord, IconSource="finance_mode"},
            new MenuOption{Title=AppResource.RefundsRecord, IconSource="currency_exchange"},
            new MenuOption{Title=AppResource.AccountsLedger, IconSource="manage_accounts"},
            new MenuOption{Title=AppResource.UploadRequest, IconSource="upload_file"},
            new MenuOption{Title=AppResource.TemporaryCredit, IconSource="credit_card"},
            new MenuOption{Title=AppResource.BankDetails, IconSource="account_balance"},
            new MenuOption{Title=AppResource.GroupQuery,IconSource="question_exchange"},
            new MenuOption{Title=AppResource.PaxCalendar,IconSource="pax_calendar"},
            new MenuOption{Title=AppResource.OnlineRecharge},
        };
    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private void ClosePopup()
    {
        MopupService.Instance.PopAsync();
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
