using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class GroupQueryPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        [ObservableProperty]
        private ObservableCollection<TabModel>? _tabs;

        [ObservableProperty]
        private TabModel? _selectedTab;

        #endregion

        public GroupQueryPageViewModel(IViewModelParameters viewModelParameters) : base(viewModelParameters)
        {

        }

        #region [ Methods & Service Calls ]

        private void InitData()
        {
            Tabs = new ObservableCollection<TabModel>
            {
                new TabModel{ Name = AppResource.DetailsTab, Icon ="detail_icon", IsSelected=true},
                new TabModel{ Name = AppResource.HistoryTab, Icon="history"}
            };
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private void TabItemSelected(object? item)
        {
            TabModel tab = (TabModel)item!;
            var previousSelectedItem = Tabs?.FirstOrDefault(x => x.IsSelected);
            previousSelectedItem?.IsSelected = false;

            //var selectedItem = Tabs?.FirstOrDefault(t => t.Name == tabName);
            tab?.IsSelected = true;
        }

        #endregion

        #region [ Override Method ]

        public override Task LoadDataWhenNavigatedTo()
        {
            InitData();
            return base.LoadDataWhenNavigatedTo();
        }

        #endregion

    }
}