using System.Windows.Input;
using NavigationMode = AirIQ.Enums.NavigationMode;

namespace AirIQ.Controls;

public partial class NavigationBarControl : ContentView
{
	public static readonly BindableProperty TitleProperty =
		BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationBarControl), string.Empty);

	public static readonly BindableProperty NavigateCommandProperty =
			BindableProperty.Create(nameof(NavigateCommand), typeof(ICommand), typeof(NavigationBarControl));

	public static readonly BindableProperty MenuCommandProperty =
				BindableProperty.Create(nameof(MenuCommand), typeof(ICommand), typeof(NavigationBarControl));

	public static readonly BindableProperty IsBackVisibleProperty =
		BindableProperty.Create(nameof(IsBackVisible), typeof(bool), typeof(NavigationBarControl), true);
	public static readonly BindableProperty IsEndIconVisibleProperty =
		BindableProperty.Create(nameof(IsEndIconVisible), typeof(bool), typeof(NavigationBarControl), true);

	public static readonly BindableProperty BackButtonTintColorProperty =
			BindableProperty.Create(nameof(BackButtonTintColor), typeof(Color), typeof(NavigationBarControl), Colors.White);

	// public static readonly BindableProperty BackgroundColorProperty =
	//    BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(NavigationBarControl), Colors.Black);

	public static readonly BindableProperty EndIconSourceProperty =
			BindableProperty.Create(nameof(EndIconSource), typeof(string), typeof(NavigationBarControl), string.Empty);

	// public static readonly BindableProperty NavigationIconProperty =
	// 	BindableProperty.Create(nameof(NavigationIcon), typeof(string), typeof(NavigationBarControl), "back_button");

	public static readonly BindableProperty NavigationModeProperty =
		BindableProperty.Create(nameof(NavigationMode), typeof(NavigationMode), typeof(NavigationBarControl), defaultValue: NavigationMode.Back, propertyChanged: OnNavigationModeChanged);



	public static readonly BindableProperty AmountProperty =
	BindableProperty.Create(nameof(Amount), typeof(double), typeof(NavigationBarControl), 0.0, BindingMode.TwoWay);


	public ICommand NavigateCommand
	{
		get => (ICommand)GetValue(NavigateCommandProperty);
		set => SetValue(NavigateCommandProperty, value);
	}

	public ICommand MenuCommand
	{
		get => (ICommand)GetValue(MenuCommandProperty);
		set => SetValue(MenuCommandProperty, value);
	}

	public bool IsBackVisible
	{
		get => (bool)GetValue(IsBackVisibleProperty);
		set => SetValue(IsBackVisibleProperty, value);
	}

	public bool IsEndIconVisible
	{
		get => (bool)GetValue(IsEndIconVisibleProperty);
		set => SetValue(IsEndIconVisibleProperty, value);
	}
	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}
	public string EndIconSource
	{
		get => (string)GetValue(EndIconSourceProperty);
		set => SetValue(EndIconSourceProperty, value);
	}

	// public string NavigationIcon
	// {
	// 	get => (string)GetValue(NavigationIconProperty);
	// 	set => SetValue(NavigationIconProperty, value);
	// }

	public Color BackButtonTintColor
	{
		get => (Color)GetValue(BackButtonTintColorProperty);
		set => SetValue(BackButtonTintColorProperty, value);
	}

	public double Amount
	{
		get => (double)GetValue(AmountProperty);
		set => SetValue(AmountProperty, value);
	}

	public NavigationMode NavigationMode
	{
		get => (NavigationMode)GetValue(NavigationModeProperty);
		set => SetValue(NavigationModeProperty, value);
	}

	private static void OnNavigationModeChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is NavigationBarControl control)
		{
			control.navigationImage.Source = control.NavigationMode == NavigationMode.Hamburger ? "menu" : "back_button";
		}
	}


	public NavigationBarControl()
	{
		InitializeComponent();
	}
}