using AirIQ.Extensions.Navigation;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using System.Diagnostics;

namespace AirIQ.Services
{
    public class NavigationService : INavigationService
    {
        readonly IServiceProvider _services;

        protected INavigation Navigation
        {
            get
            {
                INavigation? navigation = Application.Current?.MainPage.Navigation;
                if (navigation != null)
                {
                    return navigation;
                }
                else
                {
                    throw new ArgumentNullException("Failed to get the navigation service object");
                }
            }
        }

        public NavigationService(IServiceProvider services)
        {
            _services = services;
        }

        public async Task NavigateFlyoutDetail(string name, object parameter = null, bool animated = true)
        {
            try
            {
                await NavigateToFlyoutDetail(name, parameter, animated);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Failed during the navigation using nav name {exception.StackTrace}");
            }
        }

        public async Task Navigate(string name, object parameter = null, bool animated = true, bool isModal = false, bool isMainview = false)
        {
            try
            {
                await NavigateToPage(name, parameter, animated, isModal, isMainview);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Failed during the navigation using nav name {exception.StackTrace}");
            }
        }

        public async Task NavigateBack(object parameter = null, bool animated = true, bool isModal = false)
        {
            try
            {
                Page page = null;
                if (isModal)
                    page = await Navigation.PopModalAsync();
                else
                {
                    if (Application.Current?.Windows[0].Page is FlyoutPage flyoutPage)
                    {
                        // get the navigation page
                        var nav = (NavigationPage)flyoutPage.Detail;
                        // get the current displayed page
                        var CuurentPage = (ContentPage)nav.CurrentPage;

                        // navigate to the new page
                        page = await CuurentPage.Navigation.PopAsync(animated);
                        if (parameter != null)
                        {
                            var pages = nav.Navigation.NavigationStack;

                            var backwardPage = pages[pages.Count - 1];

                            await RegisterBackwardEvents(backwardPage, parameter);
                        }
                    }
                    else
                    {
                        page = await Navigation.PopAsync(animated);
                    }
                }

                await ClearContext(page);
                await DeRegisterPageEvents(page);
                await ClearUnmanagedTask(page);

                page = null;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("No pages to naviagte back!");
            }
        }

        public async Task NavigateBackToPage(string name, object parameter = null, bool animated = true, bool isModal = false)
        {
            try
            {
                var pageType = NavigationRegistry.GetPageType(name);
                if (isModal)
                {
                    foreach (var page in Navigation.ModalStack.Reverse())
                    {
                        if (page.GetType() == pageType)
                        {
                            break;
                        }
                        else
                        {
                            await ClearContext(page);
                            await DeRegisterPageEvents(page);
                            await ClearUnmanagedTask(page);
                            await Navigation.PopModalAsync(animated);
                        }
                    }
                }
                else
                {
                    foreach (var page in Navigation.NavigationStack.Reverse())
                    {
                        if (page.GetType() == pageType)
                        {
                            break;
                        }
                        else
                        {
                            await ClearContext(page);
                            await DeRegisterPageEvents(page);
                            await ClearUnmanagedTask(page);
                            await Navigation.PopAsync(animated);
                        }
                    }
                }
            }
            catch (Exception exception)
            {

                throw;
            }
        }

        public async Task NavigateToRoot()
        {
            if (Application.Current?.Windows[0].Page is FlyoutPage)
            {
                var detailPg = ((FlyoutPage)Application.Current?.Windows[0].Page).Detail;
                // get the navigation page
                var nav = (NavigationPage)detailPg;
                // get the current displayed page
                var displayPage = (ContentPage)nav.CurrentPage;

                // navigate to the new page
                await displayPage.Navigation.PopToRootAsync();
            }
            else
            {
                await Navigation.PopToRootAsync();
            }
        }

        private async Task NavigateToFlyoutDetail(string name, object parameter, bool animated)
        {
            Page toPage = (Page)NavigationRegistry.CreateView(_services, name);
            if (toPage is not null)
            {
                await RegisterEvents(toPage, parameter, false);

                if (Application.Current?.Windows[0].Page is FlyoutPage flyout)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        flyout.Detail = new NavigationPage(toPage);
                    });

                }
            }
        }

        private async Task NavigateToPage(string name, object parameter, bool animated, bool isModal, bool isMainview)
        {
            if (await CheckIfMainTab(name))
                return;

            Page toPage = (Page)NavigationRegistry.CreateView(_services, name);
            if (toPage is not null)
            {
                await RegisterFlyoutPage(toPage, parameter);

                await RegisterTabbedPage(toPage, parameter);

                await RegisterEvents(toPage, parameter, false);

                if (isMainview)
                {
                    Application.Current.MainPage = toPage is FlyoutPage ? toPage : new NavigationPage(toPage);
                }
                else
                {
                    if (isModal)
                    {
                        await Navigation.PushModalAsync(toPage, animated);
                    }
                    else
                    {

                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await Navigation.PushAsync(toPage, animated);
                        });
                    }
                }

            }
        }


        private async Task<bool> CheckIfMainTab(string name)
        {
            if (name.EndsWith("Tab"))
            {
                var tabPage = Application.Current?.Windows[0].Page.Navigation.NavigationStack.FirstOrDefault(p => p.GetType().Name.EndsWith("TabPage"));
                if (tabPage != null)
                {
                    var pages = Navigation.NavigationStack.ToList();
                    var tabIndex = pages.IndexOf(tabPage);

                    for (int maxPageIndex = pages.Count - 1; maxPageIndex > tabIndex; maxPageIndex--)
                    {
                        var currentPage = pages[maxPageIndex];
                        {
                            await ClearContext(currentPage);
                            await DeRegisterPageEvents(currentPage);
                            await ClearUnmanagedTask(currentPage);
                            Navigation.RemovePage(currentPage);
                            currentPage = null;
                        }
                    }
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        private ViewModelBase GetPageViewModelBase(Page page) =>
            page.BindingContext as ViewModelBase;

        private async Task RegisterFlyoutPage(Page toPage, object parameter)
        {
            if (toPage is FlyoutPage flyoutPage)
            {
                if (flyoutPage.Flyout is NavigationPage navigationPage)
                    await RegisterFlyoutEvents(navigationPage.CurrentPage, parameter);
                else
                    await RegisterFlyoutEvents(flyoutPage.Flyout, parameter);

                if (flyoutPage.Detail is NavigationPage navPage)
                    await RegisterFlyoutEvents(navPage.CurrentPage, parameter, true);
                else
                {
                    if (flyoutPage.Detail != null)
                        await RegisterFlyoutEvents(flyoutPage.Detail, parameter);
                }

            }
        }

        private async Task RegisterFlyoutEvents(Page page, object parameter, bool isTabbedPageChild = false)
        {
            try
            {
                await RegisterEvents(page, parameter, isTabbedPageChild);
            }
            catch (Exception exception)
            {
                throw new TaskCanceledException($"Fail to register tab page events, Execption: {exception.StackTrace}");
            }
        }

        private async Task RegisterTabbedPage(Page toPage, object parameter)
        {
            if (toPage is TabbedPage tabbed)
            {
                foreach (var page in tabbed.Children)
                {
                    if (page is NavigationPage tabPage)
                    {
                        await RegisterTabbedPageEvents(tabPage.CurrentPage, parameter);
                    }
                    else
                    {
                        await RegisterTabbedPageEvents(page, parameter);
                    }
                }
            }
        }

        private async Task RegisterTabbedPageEvents(Page page, object parameter)
        {
            try
            {
                await RegisterEvents(page, parameter, true);
            }
            catch (Exception exception)
            {
                throw new TaskCanceledException($"Fail to register tab page events, Execption: {exception.StackTrace}");
            }
        }


        private async Task RegisterBackwardEvents(Page toPage, object parameter)
        {
            var toViewModel = GetPageViewModelBase(toPage);
            if (toViewModel is null)
                return;
            await toViewModel.OnBackNavigatingTo(parameter);
        }

        private async Task RegisterEvents(Page toPage, object parameter, bool isTabbedPageChild)
        {
            toPage.NavigatedTo += ToPage_NavigatedTo;

            var toViewModel = GetPageViewModelBase(toPage);
            if (toViewModel is null)
                return;

            await toViewModel.OnNavigatedTo(parameter);


            if (isTabbedPageChild)
                await toViewModel.OnNavigatedTo();

            //toPage.NavigatedFrom += ToPage_NavigatedFrom;
            toPage.Appearing += ToPage_Appearing;
            toPage.Disappearing += ToPage_Disappearing;
        }

        private async void ToPage_Disappearing(object sender, EventArgs e)
        {
            if (sender is Page thisPage)
            {
                await CallDisappearing(thisPage);
            }
        }

        private async void ToPage_Appearing(object sender, EventArgs e)
        {
            if (sender is Page thisPage)
            {
                await CallAppearing(thisPage);
            }
        }


        private async void ToPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            //To determine forward navigation, we look at the 2nd to last item on the NavigationStack
            //If that entry equals the sender, it means we navigated forward from the sender to another page
            bool isForwardNavigation = Navigation.NavigationStack.Count > 1 && Navigation.NavigationStack[^2] == sender;

            if (sender is Page thisPage)
            {
                if (!isForwardNavigation)
                    thisPage.NavigatedFrom -= ToPage_NavigatedFrom;

                await CallNavigatedFrom(thisPage, isForwardNavigation);
            }

        }

        private async void ToPage_NavigatedTo(object sender, NavigatedToEventArgs e)
           => await CallNavigatedTo(sender as Page);

        private Task CallNavigatedTo(Page page)
        {
            var fromViewModel = GetPageViewModelBase(page);
            if (fromViewModel is not null)
            {
                return fromViewModel.OnNavigatedTo();
            }

            return Task.CompletedTask;
        }

        private Task CallNavigatedFrom(Page thisPage, bool isForwardNavigation)
        {
            var fromViewModel = GetPageViewModelBase(thisPage);
            if (fromViewModel is not null)
            {
                return fromViewModel.OnNavigatedFrom(isForwardNavigation);
            }

            return Task.CompletedTask;
        }

        private Task CallAppearing(Page thisPage)
        {
            var fromViewModel = GetPageViewModelBase(thisPage);
            if (fromViewModel is not null)
            {
                return fromViewModel.OnViewAppearing();
            }

            return Task.CompletedTask;
        }
        private Task CallDisappearing(Page thisPage)
        {
            var fromViewModel = GetPageViewModelBase(thisPage);
            if (fromViewModel is not null)
            {
                return fromViewModel.OnViewDisappearing();
            }

            return Task.CompletedTask;
        }

        private Task ClearContext(Page page)
        {
            if (page.BindingContext != null)
            {
                IDestructible destructible = page.BindingContext as IDestructible;
                destructible.Destory();
            }
            return Task.CompletedTask;
        }
        private Task DeRegisterPageEvents(Page page)
        {
            page.Appearing -= ToPage_Appearing;
            page.Disappearing -= ToPage_Disappearing;
            page.NavigatedFrom -= ToPage_NavigatedFrom;
            page.NavigatedTo -= ToPage_NavigatedTo;


            return Task.CompletedTask;
        }
        private Task ClearUnmanagedTask(Page page)
        {
            try
            {
                if (page is IDisposable)
                {
                    IDisposable disposable = page as IDisposable;
                    disposable.Dispose();
                }

            }
            catch (Exception exception)
            {
                Debug.WriteLine($"Failed to dispose {exception.StackTrace}");
            }


            return Task.CompletedTask;
        }
    }
}
