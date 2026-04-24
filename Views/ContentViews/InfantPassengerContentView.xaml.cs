using System.Windows.Input;

namespace AirIQ.Views.ContentViews;

public partial class InfantPassengerContentView : ContentView
{
	public static readonly BindableProperty SaveCommandProperty =
		BindableProperty.Create(nameof(SaveCommand), typeof(ICommand), typeof(InfantPassengerContentView));

	public ICommand SaveCommand
	{
		get => (ICommand)GetValue(SaveCommandProperty);
		set => SetValue(SaveCommandProperty, value);
	}
	public InfantPassengerContentView()
	{
		InitializeComponent();
	}
}