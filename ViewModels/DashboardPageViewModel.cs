using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirIQ.ViewModels
{
    public partial class DashboardPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        [ObservableProperty]
        private ObservableCollection<string> _sourceAirports = new ObservableCollection<string> { "Mumbai", "Bangalore", "Chennai" };

        [ObservableProperty]
        private ObservableCollection<string> _destinationAirports = new ObservableCollection<string> { "Chennai", "Mumbai", "Bangalore" };

        #endregion


    }
}
