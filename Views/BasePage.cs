using AirIQ.Controls;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Microsoft.Maui.Controls.Application;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;
using NavigationMode = AirIQ.Enums.NavigationMode;
using AirIQ.Configurations;

namespace AirIQ.Views;

public abstract class BasePage : ContentPage
{
	private Color StatusBarColor => (Color)(Application.Current?.Resources["PrimaryColor"] ?? Colors.Black);
	private readonly ContentView _content;
	private readonly NavigationBarControl _navBar;
	protected BasePage()
	{
		Shell.SetNavBarIsVisible(this, false);
		NavigationPage.SetHasNavigationBar(this, false);

#if IOS
		ApplyStatusBarStyle();
#endif

		var layout = new Grid
		{
			RowDefinitions =
			{
				new RowDefinition { Height = GridLength.Auto },
				new RowDefinition { Height = GridLength.Star  }
			}
		};
		_navBar = new Controls.NavigationBarControl();
		_navBar.SetBinding(Controls.NavigationBarControl.TitleProperty, new Binding(nameof(PageTitle), source: this));
		_navBar.SetBinding(Controls.NavigationBarControl.IsBackVisibleProperty, new Binding(nameof(IsBackVisible), source: this));
		_navBar.SetBinding(Controls.NavigationBarControl.NavigateCommandProperty, new Binding("NavigateCommand", source: this.BindingContext));
		_navBar.SetBinding(NavigationBarControl.NavigationModeProperty, new Binding(nameof(NavigationMode), source: this));
		_navBar.SetBinding(NavigationBarControl.IsWalletVisibleProperty, binding: new Binding(nameof(IsWalletVisible), source: this));

		_content = new ContentView();
		_content.BackgroundColor = Colors.White;



		_navBar.BackgroundColor = (Color)Application.Current?.Resources["PrimaryColor"]!;
		//_navBar.BackButtonTintColor = Color.FromArgb("#1C1C1C");

		layout.Add(_navBar);
		Grid.SetRow(_navBar, 0);

		layout.Add(_content);
		Grid.SetRow(_content, 1);

		this.Content = layout;

	}

	#region [ Bindable Properties ]

	public static readonly BindableProperty PageContentProperty =
		BindableProperty.Create(nameof(PageContent), typeof(View), typeof(BasePage), propertyChanged: OnPageContentChanged);

	public static readonly BindableProperty IsNavBarVisibleProperty =
		BindableProperty.Create(nameof(IsNavBarVisible), typeof(bool), typeof(BasePage), true);

	public static readonly BindableProperty PageTitleProperty =
		BindableProperty.Create(nameof(PageTitle), typeof(string), typeof(BasePage), string.Empty);

	public static readonly BindableProperty IsBackVisibleProperty =
		BindableProperty.Create(nameof(IsBackVisible), typeof(bool), typeof(BasePage), true);

	public static readonly BindableProperty NavigationModeProperty =
		BindableProperty.Create(nameof(NavigationMode), typeof(NavigationMode), typeof(BasePage), defaultValue: NavigationMode.Back);

	public static readonly BindableProperty IsWalletVisibleProperty =
		BindableProperty.Create(nameof(IsWalletVisible), typeof(bool), typeof(BasePage), defaultValue: true);

	public string PageTitle
	{
		get => (string)GetValue(PageTitleProperty);
		set => SetValue(PageTitleProperty, value);
	}

	public bool IsBackVisible
	{
		get => (bool)GetValue(IsBackVisibleProperty);
		set => SetValue(IsBackVisibleProperty, value);
	}

	public bool IsWalletVisible
	{
		get => (bool)GetValue(IsWalletVisibleProperty);
		set => SetValue(IsWalletVisibleProperty, value);
	}

	public bool IsNavBarVisible
	{
		get => (bool)GetValue(IsNavBarVisibleProperty);
		set
		{
			SetValue(IsNavBarVisibleProperty, value);
		}
	}

	public View PageContent
	{
		get => (View)GetValue(PageContentProperty);
		set => SetValue(PageContentProperty, value);
	}

	public NavigationMode NavigationMode
	{
		get => (NavigationMode)GetValue(NavigationModeProperty);
		set => SetValue(NavigationModeProperty, value);
	}
	private static void OnPageContentChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is BasePage basePage && newValue is View newContent)
		{
			basePage._content.Content = newContent;
		}
	}

	#endregion

	#region [ Methods ]

	private void ApplyStatusBarStyle()
	{
		if (this.GetType() == typeof(DashboardPage))
		{
			this.Behaviors.Add(new StatusBarBehavior
			{
				StatusBarColor = Color.FromArgb("#4D9DF0"),
				StatusBarStyle = StatusBarStyle.LightContent
			});
		}
		else
		{
			this.Behaviors.Add(new StatusBarBehavior
			{
				StatusBarColor = StatusBarColor,
				StatusBarStyle = StatusBarStyle.LightContent
			});
		}


		if (OperatingSystem.IsIOS())
		{
			var safeInsects = On<iOS>().SafeAreaInsets();
			if (safeInsects.Top <= 0)
			{
				On<iOS>().SetUseSafeArea(true);
			}
			else
			{
				this.Padding = new Thickness(0, safeInsects.Top, 0, 0);
			}
		}
	}
	#endregion

	#region [ Override Methods ]



	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);
		if (this.BindingContext is BaseViewModel vm &&
			(args.NavigationType == NavigationType.Push ||
			 args.NavigationType == NavigationType.Replace))
		{
			_ = vm.LoadDataWhenNavigatedTo();
		}
	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (_navBar != null)
		{
			_navBar.IsVisible = IsNavBarVisible;
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (this.BindingContext is BaseViewModel vm)
		{
			_ = vm.LoadDataWhenOnAppearing();
		}
#if ANDROID
		if (Platform.CurrentActivity is MainActivity activity)
		{
			activity.ApplySystemBars(this);
		}
#endif

		_navBar.Amount = AppConfiguration.CurrentUser?.Balance ?? 0;
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		if (this.BindingContext is BaseViewModel vm)
		{
			_ = vm.LoadDataWhenOnDisappearing();
		}
	}

	protected override bool OnBackButtonPressed()
	{
		return base.OnBackButtonPressed();
	}

	#endregion
}