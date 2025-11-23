using System.Windows.Input;

namespace AirIQ.Controls;

public partial class NavigationBarControl : ContentView
{
	public static readonly BindableProperty TitleProperty =
		BindableProperty.Create(nameof(Title), typeof(string), typeof(NavigationBarControl), string.Empty);

	public static readonly BindableProperty BackCommandProperty =
			BindableProperty.Create(nameof(BackCommand), typeof(ICommand), typeof(NavigationBarControl));

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


	public ICommand BackCommand
	{
		get => (ICommand)GetValue(BackCommandProperty);
		set => SetValue(BackCommandProperty, value);
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
	public Color BackButtonTintColor
	{
		get => (Color)GetValue(BackButtonTintColorProperty);
		set => SetValue(BackButtonTintColorProperty, value);
	}


	public NavigationBarControl()
	{
		InitializeComponent();
	}
}