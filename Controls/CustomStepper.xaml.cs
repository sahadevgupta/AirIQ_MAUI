namespace AirIQ.Controls;

public partial class CustomStepper : ContentView
{
	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(int), typeof(CustomStepper), 01, BindingMode.TwoWay);

	public int Value
	{
		get => (int)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}
	public CustomStepper()
	{
		InitializeComponent();
	}

	private void minus_Clicked(object sender, EventArgs e)
	{
		if (Value > 1)
			Value--;
	}

	private void plus_Clicked(object sender, EventArgs e)
	{
		Value++;
	}
}