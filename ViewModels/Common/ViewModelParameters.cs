using AirIQ.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirIQ.ViewModels.Common
{
    public class ViewModelParameters : IViewModelParameters
    {
        public ViewModelParameters(ILoadingPopUpService loadingPopUpService, 
            INavigationService navigationService,
            IShellNavigationService shellNavigationService)
        {
            LoadingPopUpService = loadingPopUpService;
            NavigationService = navigationService;
            ShellNavigationService = shellNavigationService;
        }
        public ILoadingPopUpService LoadingPopUpService { get; }
        public INavigationService NavigationService { get; }
        public IShellNavigationService ShellNavigationService { get; }
    }
}
