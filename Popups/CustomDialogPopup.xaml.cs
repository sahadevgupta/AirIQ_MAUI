using System.ComponentModel;
using AirIQ.Extensions;
using Mopups.Pages;

namespace AirIQ.Popups;

public partial class CustomDialogPopup : PopupPage
{
	public static BindableProperty MessageProperty =
	BindableProperty.Create(nameof(Message), typeof(string), typeof(CustomDialogPopup), null, BindingMode.TwoWay);

	/// <summary>
	/// set Icon for the control
	/// </summary>
	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(CustomDialogPopup), FontAwesomeIcons.ChevronDown, BindingMode.TwoWay);

	/// <summary>
	/// Identifies the <see cref="IconTintColorProperty"/> bindable property.
	/// </summary>
	public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(
		nameof(IconTintColor),
		typeof(Color),
		typeof(CustomDialogPopup),
		default(Color));

	public string Message
	{
		get => (string)GetValue(MessageProperty);
		set => SetValue(MessageProperty, value);
	}
	public Color IconTintColor
	{
		get => (Color)GetValue(IconTintColorProperty);
		set => SetValue(IconTintColorProperty, value);
	}

	[TypeConverter(typeof(ImageSource))]
	public string Icon
	{
		get { return (string)GetValue(IconProperty); }
		set { SetValue(IconProperty, value); }
	}
	public CustomDialogPopup()
	{
		InitializeComponent();
	}
}