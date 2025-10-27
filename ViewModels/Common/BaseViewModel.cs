using AirIQ.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.ViewModels.Common
{
    public partial class BaseViewModel : ViewModelBase, IDestructible
    {
        [ObservableProperty]
        private bool _isBusy;
    }
}
