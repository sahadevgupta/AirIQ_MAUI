namespace AirIQ.Controls;

public partial class CustomStepper : ContentView
{
	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(int), typeof(CustomStepper), 01, BindingMode.TwoWay);

	public static readonly BindableProperty MinValueProperty =
		BindableProperty.Create(nameof(MinValue), typeof(int), typeof(CustomStepper), 1);

	public static readonly BindableProperty MaxValueProperty =
		BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(CustomStepper), int.MaxValue);

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

	public int MaxValue
	{
		get => (int)GetValue(MaxValueProperty);
		set => SetValue(MaxValueProperty, value);
	}

	// Tracks the last value that was within [MinValue, MaxValue] so free-text entry
	// can be reverted if the user leaves the field with invalid/out-of-range text.
	private int _lastValidValue;

	public CustomStepper()
	{
		InitializeComponent();
		_lastValidValue = Value;
	}

	private void minus_Clicked(object sender, EventArgs e)
	{
		if (Value > MinValue)
			Value--;
	}

	private void plus_Clicked(object sender, EventArgs e)
	{
		if (Value < MaxValue)
			Value++;
	}

	private void valueEntry_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (int.TryParse(e.NewTextValue, out var typed) && typed >= MinValue && typed <= MaxValue)
			_lastValidValue = typed;
	}

	private void valueEntry_Unfocused(object sender, FocusEventArgs e)
	{
		if (!int.TryParse(valueEntry.Text, out var typed))
		{
			Value = _lastValidValue;
			return;
		}

		var clamped = Math.Clamp(typed, MinValue, MaxValue);
		Value = clamped;
		_lastValidValue = clamped;
	}
}