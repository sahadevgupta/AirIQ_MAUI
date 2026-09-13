using Mopups.Pages;
using Mopups.Services;

namespace AirIQ.Popups;

public partial class SearchFilterPopup : PopupPage
{
	public static readonly BindableProperty FromDateProperty =
		BindableProperty.Create(nameof(FromDate), typeof(DateTime?), typeof(SearchFilterPopup), null, BindingMode.TwoWay, propertyChanged: OnFromDateChanged);

	public static readonly BindableProperty ToDateProperty =
		BindableProperty.Create(nameof(ToDate), typeof(DateTime?), typeof(SearchFilterPopup), null, BindingMode.TwoWay, propertyChanged: OnToDateChanged);

	static readonly Color SelectedChipBackground = (Color)Application.Current!.Resources["Primary0"];
	static readonly Color SelectedChipStroke = (Color)Application.Current!.Resources["PrimaryColor"];
	static readonly Color SelectedChipText = (Color)Application.Current!.Resources["PrimaryColor"];
	static readonly Color UnselectedChipStroke = (Color)Application.Current!.Resources["BorderColor"];
	static readonly Color UnselectedChipText = (Color)Application.Current!.Resources["Onyx"];

	public DateTime? FromDate
	{
		get => (DateTime?)GetValue(FromDateProperty);
		set => SetValue(FromDateProperty, value);
	}

	public DateTime? ToDate
	{
		get => (DateTime?)GetValue(ToDateProperty);
		set => SetValue(ToDateProperty, value);
	}

	public event Action<DateTime?, DateTime?>? Applied;

	public SearchFilterPopup()
	{
		InitializeComponent();
	}

	static void OnFromDateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var popup = (SearchFilterPopup)bindable;
		popup.UpdateDateLabel(popup.fromDateLabel, (DateTime?)newValue);
	}

	static void OnToDateChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var popup = (SearchFilterPopup)bindable;
		popup.UpdateDateLabel(popup.toDateLabel, (DateTime?)newValue);
	}

	void UpdateDateLabel(Label label, DateTime? date)
	{
		if (date.HasValue)
		{
			label.Text = date.Value.ToString("dd/MM/yyyy");
			label.TextColor = (Color)Application.Current!.Resources["Onyx"];
		}
		else
		{
			label.Text = AirIQ.Resources.Strings.AppResource.DateFormatPlaceholder;
			label.TextColor = (Color)Application.Current!.Resources["Gray400"];
		}
	}

	void FromDateTapped(object? sender, EventArgs e)
	{
		fromDatePicker.Date = FromDate ?? DateTime.Today;
		OpenDatePicker(fromDatePicker);
	}

	void ToDateTapped(object? sender, EventArgs e)
	{
		toDatePicker.Date = ToDate ?? FromDate ?? DateTime.Today;
		OpenDatePicker(toDatePicker);
	}

	static void OpenDatePicker(DatePicker datePicker)
	{
#if ANDROID
		var handler = datePicker.Handler as Microsoft.Maui.Handlers.IDatePickerHandler;
		handler?.PlatformView.PerformClick();
#else
		datePicker.Focus();
#endif
	}

	void FromDatePicker_Closed(object? sender, DatePickerClosedEventArgs e)
	{
		FromDate = fromDatePicker.Date;

		if (ToDate.HasValue && ToDate.Value < FromDate!.Value)
			ToDate = FromDate;

		ClearPresetSelection();
	}

	void ToDatePicker_Closed(object? sender, DatePickerClosedEventArgs e)
	{
		ToDate = toDatePicker.Date;

		if (FromDate.HasValue && ToDate!.Value < FromDate.Value)
			FromDate = ToDate;

		ClearPresetSelection();
	}

	void PresetTapped(object? sender, TappedEventArgs e)
	{
		if (e.Parameter is not string preset)
			return;

		var today = DateTime.Today;

		(DateTime from, DateTime to) = preset switch
		{
			"Today" => (today, today),
			"Week" => (today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1)), today),
			"Month" => (new DateTime(today.Year, today.Month, 1), today),
			"Last30" => (today.AddDays(-29), today),
			_ => (today, today)
		};

		FromDate = from;
		ToDate = to;

		HighlightPreset(preset);
	}

	void HighlightPreset(string preset)
	{
		SetChipSelected(todayChip, preset == "Today");
		SetChipSelected(weekChip, preset == "Week");
		SetChipSelected(monthChip, preset == "Month");
		SetChipSelected(last30Chip, preset == "Last30");
	}

	void ClearPresetSelection() => HighlightPreset(string.Empty);

	static void SetChipSelected(Border chip, bool isSelected)
	{
		chip.BackgroundColor = isSelected ? SelectedChipBackground : Colors.White;
		chip.Stroke = isSelected ? SelectedChipStroke : UnselectedChipStroke;

		if (chip.Content is Label label)
			label.TextColor = isSelected ? SelectedChipText : UnselectedChipText;
	}

	void ClearClicked(object? sender, EventArgs e)
	{
		FromDate = null;
		ToDate = null;
		ClearPresetSelection();
	}

	async void CloseTapped(object? sender, EventArgs e)
	{
		await MopupService.Instance.PopAsync();
	}

	async void SearchClicked(object? sender, EventArgs e)
	{
		Applied?.Invoke(FromDate, ToDate);
		await MopupService.Instance.PopAsync();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		Applied = null;
	}
}
