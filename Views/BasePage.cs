using AirIQ.Controls;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using Application = Microsoft.Maui.Controls.Application;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;

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
		ApplyStatusBarStyle();

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
		_navBar.SetBinding(Controls.NavigationBarControl.BackCommandProperty, new Binding("BackCommand", source: this.BindingContext));

		_content = new ContentView();
		_content.BackgroundColor = (Color)(Application.Current?.Resources["BackgroundColor"] ?? Colors.White);



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
				StatusBarColor = Color.FromArgb("#D0E1FD"),
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
		if (this.BindingContext is BaseViewModel vm && args.NavigationType != NavigationType.Pop)
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
		if (this.GetType() == typeof(DashboardPage))
		{
			if (Platform.CurrentActivity is MainActivity activity)
			{
				activity.ApplySystemBars(this);
			}
			this.Background = (Brush)Application.Current?.Resources["BrandGradient"]!;
		}
#endif
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		if (this.BindingContext is BaseViewModel vm)
		{
			_ = vm.LoadDataWhenOnDisappearing();
		}
	}

	#endregion
}