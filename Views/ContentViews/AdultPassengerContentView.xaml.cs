using System.Windows.Input;

namespace AirIQ.Views.ContentViews;

public partial class AdultPassengerContentView : ContentView
{
	public static readonly BindableProperty SaveCommandProperty =
		BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(AdultPassengerContentView));

	public ICommand SaveCommand
	{
		get => (ICommand)GetValue(SaveCommandProperty);
		set => SetValue(SaveCommandProperty, value);
	}
	public AdultPassengerContentView()
	{
		InitializeComponent();
	}
}