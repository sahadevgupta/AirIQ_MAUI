using AirIQ.Enums;

namespace AirIQ.Controls;

public partial class DateChip : ContentView
{
	public DateChip()
	{
		InitializeComponent();
		TapGesture = new TapGestureRecognizer();
		TapGesture.Tapped += (s, e) => ChipTapped?.Invoke(this, EventArgs.Empty);

		GestureRecognizers.Add(TapGesture);
	}

	public event EventHandler ChipTapped;

	public TapGestureRecognizer TapGesture { get; set; }

	public static readonly BindableProperty TextProperty =
		BindableProperty.Create(nameof(Text), typeof(string), typeof(DateChip), default(string));
	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly BindableProperty IsSelectedProperty =
		BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(DateChip), false, propertyChanged: OnSelectChanged);
	public bool IsSelected
	{
		get => (bool)GetValue(IsSelectedProperty);
		set => SetValue(IsSelectedProperty, value);
	}

	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(DateChip), default(ImageSource));
	public ImageSource Icon
	{
		get => (ImageSource)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly BindableProperty ShowIconProperty =
		BindableProperty.Create(nameof(ShowIcon), typeof(bool), typeof(DateChip), false);
	public bool ShowIcon
	{
		get => (bool)GetValue(ShowIconProperty);
		set => SetValue(ShowIconProperty, value);
	}

	public static readonly BindableProperty ImageAlingmentProperty =
		BindableProperty.Create(nameof(ImageAlingment), typeof(AlingmentType), typeof(DateChip), AlingmentType.Left);

	public AlingmentType ImageAlingment
	{
		get => (AlingmentType)GetValue(ImageAlingmentProperty);
		set => SetValue(ImageAlingmentProperty, value);
	}

	public Color ChipBackground => IsSelected ? Color.FromArgb("#E6F1FF") : Colors.White;
	public Color BorderColor => IsSelected ? Color.FromArgb("#1A73E8") : Color.FromArgb("#D3D3D3");
	public Color TextColor => IsSelected ? Color.FromArgb("#1A73E8") : Colors.Black;

	private static void OnSelectChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (DateChip)bindable;
		control.OnPropertyChanged(nameof(ChipBackground));
		control.OnPropertyChanged(nameof(BorderColor));
		control.OnPropertyChanged(nameof(TextColor));
	}
}