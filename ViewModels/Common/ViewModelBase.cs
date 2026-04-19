using CommunityToolkit.Mvvm.ComponentModel;

namespace AirIQ.ViewModels.Common
{
    public abstract class ViewModelBase : ObservableObject
    {
        public IDictionary<string, object> NavigationParameters { get; set; }

        public bool IsLoaded { get; set; }

        public Task OnViewAppearing()
        {
            return OnPageAppearing();

        }

        public Task OnViewDisappearing()
        {
            return OnPageDisappearing();
        }

        private async Task OnPageDisappearing()
        {
            await LoadDataWhenOnDisappearing();
        }

        private async Task OnPageAppearing()
        {
            await LoadDataWhenOnAppearing();
        }

        public virtual Task OnNavigatedTo(object parameter)
        {
            NavigationParameters = parameter as IDictionary<string, object>;
            return Task.CompletedTask;
        }

        public async Task OnBackNavigatingTo(object parameter)
        {
            NavigationParameters = parameter as IDictionary<string, object>;
            await LoadDataWhenBackNavigatingTo(NavigationParameters);
        }

        public virtual Task LoadDataWhenBackNavigatingTo(IDictionary<string, object> navigationParameters)
        {
            return Task.CompletedTask;
        }

        public async virtual Task OnNavigatedTo()
        {
            if (!IsLoaded)
            {
                await LoadDataWhenNavigatingTo(NavigationParameters);
                IsLoaded = true;
            }
        }
        public async virtual Task OnNavigatedFrom(bool isForwardNavigation)
        {
            if (!IsLoaded)
            {
                await LoadDataWhenOnAppearing();
                IsLoaded = true;
            }
        }

        public virtual void Destory()
        {
            NavigationParameters = null;
        }

        public virtual Task LoadDataWhenNavigatingTo(IDictionary<string, object> navigationParameters)
        {
            return Task.CompletedTask;
        }
        public virtual Task LoadDataWhenOnAppearing()
        {
            return Task.CompletedTask;
        }
        public virtual Task LoadDataWhenOnDisappearing()
        {
            return Task.CompletedTask;
        }
    }
}
