namespace AirIQ.Controls;

public partial class CustomStepper : ContentView
{
	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(int), typeof(CustomStepper), 01, BindingMode.TwoWay);

	public static readonly BindableProperty MinValueProperty =
		BindableProperty.Create(nameof(MinValue), typeof(int), typeof(CustomStepper), 1);

	public int Value
	{
		get => (int)GetValue(ValueProperty);
		set => SetValue(ValueProperty, value);
	}

	public int MinValue
	{
		get => (int)GetValue(MinValueProperty);
		set => SetValue(MinValueProperty, value);
	}
	public CustomStepper()
	{
		InitializeComponent();
	}

	private void minus_Clicked(object sender, EventArgs e)
	{
		if (Value > MinValue)
			Value--;
	}

	private void plus_Clicked(object sender, EventArgs e)
	{
		Value++;
	}
}