using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirIQ.Extensions.Navigation
{
    public static class NavigationRegistry
    {
        static readonly List<ViewRegistration> _registrations = new List<ViewRegistration>();

        public static void Register<TView, TViewModel>(string name) =>
            Register(typeof(TView), typeof(TViewModel), name);

        public static Type GetPageType(string name) =>
            _registrations.FirstOrDefault(x => x.Name == name)?.View;

        private static void Register(Type viewType, Type viewModelType, string name)
        {
            if (_registrations.Exists(x => x.Name == name))
                throw new InvalidNavigationException($"A view with the same name '{name}' has already been register");

            var registration = new ViewRegistration
            {
                View = viewType,
                ViewModel = viewModelType,
                Name = name
            };
            _registrations.Add(registration);
        }
        public static object CreateView(IServiceProvider services, string name)
        {
            ViewRegistration registration = GetViewRegistration(name);
            var view = services.GetService(registration.View);

            if (view is null)
                throw new KeyNotFoundException($"No view with the name '{name}' has been registered");

            RegisterTabbedPage(services, view);
            RegisterFlyoutPage(services, view);
            Bind(services, registration, view);

            return view;
        }

        private static ViewRegistration GetViewRegistration(string name)
        {
            var registration = _registrations.FirstOrDefault(x => x.Name == name);
            if (registration is null)
                throw new KeyNotFoundException($"No view with the name '{name}' has been registered");
            return registration;
        }

        private static ViewRegistration GetViewRegistration(Type T)
        {
            var registration = _registrations.FirstOrDefault(x => x.View == T);
            if (registration is null)
                throw new KeyNotFoundException($"No view with the name '{T.Name}' has been registered");
            return registration;
        }

        private static void Bind(IServiceProvider services, ViewRegistration registration, object view, bool isForceSetBindingContext = false)
        {
            if (isForceSetBindingContext)
            {
                if (view is BindableObject bindable)
                {
                    var vm = services.GetPageModel(registration.ViewModel);
                    if (vm != null)
                        bindable.BindingContext = vm;
                    else
                        throw new FileNotFoundException("ViewModel not registered " + view.ToString());
                }
            }
            else
            {
                if (view is BindableObject bindable && bindable.BindingContext is null)
                {
                    var vm = services.GetPageModel(registration.ViewModel);
                    if (vm != null)
                    {
                            bindable.BindingContext = vm;
                    }

                    else
                        throw new FileNotFoundException("ViewModel not registered " + view.ToString());
                }
            }

        }

        private static void RegisterTabbedPage(IServiceProvider services, object view)
        {
            if (view is TabbedPage tabbedPage && tabbedPage.Children != null)
            {
                foreach (var page in tabbedPage.Children)
                {
                    if (page is NavigationPage navPage)
                    {
                        navPage.CurrentPage.BindingContext = null;
                        ViewRegistration tabItemRegistration = GetViewRegistration(navPage.CurrentPage.GetType());
                        Bind(services, tabItemRegistration, navPage.CurrentPage);
                    }
                    else
                    {
                        page.BindingContext = null;
                        ViewRegistration tabItemRegistration = GetViewRegistration(page.GetType());
                        Bind(services, tabItemRegistration, page);
                    }
                }
            }
        }

        private static void RegisterFlyoutPage(IServiceProvider services, object view)
        {
            if (view is FlyoutPage flyoutPage)
            {
                if (flyoutPage.Detail is NavigationPage navigationPage)
                {
                    var page = navigationPage.CurrentPage;
                    ViewRegistration detailRegistration = GetViewRegistration(navigationPage.CurrentPage.GetType());
                    page.BindingContext = null;
                    Bind(services, detailRegistration, page, true);
                }

                //if (flyoutPage.Flyout is ContentPage contentPage)
                //{
                //    contentPage.BindingContext = null;
                //    ViewRegistration flyoutRegistration = GetViewRegistration(contentPage.GetType());
                //    Bind(services, flyoutRegistration, contentPage);
                //}
            }
        }
    }
}
