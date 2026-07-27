using System.ComponentModel;
using System.Windows.Input;
using AirIQ.Extensions;
using Mopups.Pages;
using Mopups.Services;

namespace AirIQ_MAUI.Popups;

public partial class CustomAlertPopup : PopupPage
{
	public static BindableProperty MessageProperty =
	BindableProperty.Create(nameof(Message), typeof(string), typeof(CustomAlertPopup), null, BindingMode.TwoWay);

	/// <summary>
	/// set Icon for the control
	/// </summary>
	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(CustomAlertPopup), FontAwesomeIcons.ChevronDown, BindingMode.TwoWay);

	/// <summary>
	/// Identifies the <see cref="IconTintColorProperty"/> bindable property.
	/// </summary>
	public static readonly BindableProperty IconTintColorProperty = BindableProperty.Create(
		nameof(IconTintColor),
		typeof(Color),
		typeof(CustomAlertPopup),
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

	public CustomAlertPopup()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await MopupService.Instance.PopAsync();
	}
}