namespace AirIQ.Controls;

public partial class DateSelector : ContentView
{
	public DateSelector()
	{
		InitializeComponent();

		// Tap handlers inside the control
		LeftChip.ChipTapped += (s, e) => MoveLeft();
		RightChip.ChipTapped += (s, e) => MoveRight();
		CenterChip.ChipTapped += (s, e) => { /* center already selected */ };
	}

	// Bindable SelectedDate property
	public static readonly BindableProperty SelectedDateProperty =
		BindableProperty.Create(
			nameof(SelectedDate),
			typeof(DateTime),
			typeof(DateSelector),
			default(DateTime),
			BindingMode.TwoWay,
			propertyChanged: OnDateChanged);

	public DateTime SelectedDate
	{
		get => (DateTime)GetValue(SelectedDateProperty);
		set => SetValue(SelectedDateProperty, value);
	}

	private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		((DateSelector)bindable).UpdateChips();
	}

	// Update chip text and selection
	void UpdateChips()
	{
		LeftChip.Text = SelectedDate.AddDays(-1).ToString("dd MMM, yyyy");
		CenterChip.Text = SelectedDate.ToString("dd MMM, yyyy");
		RightChip.Text = SelectedDate.AddDays(1).ToString("dd MMM, yyyy");

		LeftChip.IsSelected = false;
		RightChip.IsSelected = false;
		CenterChip.IsSelected = true;
	}

	void MoveLeft()
	{
		SelectedDate = SelectedDate.AddDays(-1);
	}

	void MoveRight()
	{
		SelectedDate = SelectedDate.AddDays(1);
	}
}