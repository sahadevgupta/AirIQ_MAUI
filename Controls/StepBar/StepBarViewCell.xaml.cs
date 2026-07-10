using System.ComponentModel;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.ViewModels.Common;

namespace AirIQ.Controls.StepBar;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class StepBarViewCell : ContentView
{
	private StepBarModel? _previousModel;
	private int stepCount;
	// Content for each step
	public static readonly BindableProperty StepContentProperty =
		BindableProperty.Create(nameof(StepContent), typeof(View), typeof(StepBarViewCell));

	public View StepContent
	{
		get => (View)GetValue(StepContentProperty);
		set => SetValue(StepContentProperty, value);
	}

	// Event to notify parent that a step is tapped
	public event Action<View>? StepSelected;
	public StepBarViewCell(int totalStepCount)
	{
		InitializeComponent();
		this.stepCount = totalStepCount;
		if (Device.RuntimePlatform == Device.iOS)
		{
			progress.SetBinding(ProgressBar.ProgressProperty, "ProgressValue");
		}
		else
		{
			progress.SetBinding(ProgressBar.ProgressProperty, "ProgressValue", BindingMode.TwoWay);
		}

	}
	protected override void OnSizeAllocated(double width, double height)
	{
		base.OnSizeAllocated(width, height);

		var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
		double cellwidth = screenWidth / stepCount;
		progress.WidthRequest = cellwidth;
		// mainLabel.WidthRequest = cellwidth - 8 - 20;
		//if (BindingContext is StepBarModel selectedmodel)
		//{
		//    if (selectedmodel.IsNotLast)
		//    {

		//        progress.WidthRequest = this.WidthRequest - 20;
		//        trackergrid.HorizontalOptions = LayoutOptions.CenterAndExpand;
		//    }
		//    else
		//    {
		//        this.WidthRequest = 100;
		//        progress.Margin = new Thickness(-80, 10, 0, 0);
		//        //trackergrid.HorizontalOptions = LayoutOptions.EndAndExpand;
		//    }
		//}

	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();

		// Unsubscribe old context
		if (_previousModel != null)
			_previousModel.PropertyChanged -= Model_PropertyChanged;

		if (BindingContext is StepBarModel selectedmodel)
		{
			_previousModel = selectedmodel;
			selectedmodel.PropertyChanged += Model_PropertyChanged;

			if (!selectedmodel.IsNotLast)
			{
				//mainGrid.ColumnDefinitions.RemoveAt(0);
			}

			if (selectedmodel.IsCurrentContent)
			{
				StepContent = selectedmodel.MainContent!;
				StepSelected?.Invoke(selectedmodel.MainContent!);
			}

			var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.Current.MainDisplayInfo.Density;
			double cellwidth = screenWidth / stepCount;


			progress.WidthRequest = cellwidth;
		}

	}
	private async void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		var model = (StepBarModel)sender;
		if (e.PropertyName == nameof(StepBarModel.Status))
		{
			switch (model.Status)
			{
				case StepBarStatus.Completed:
					await AnimateProgress();
					break;

				case StepBarStatus.InProgress:
					StepSelected?.Invoke(model.MainContent);
					progress.Progress = 0;
					break;

				default:
					progress.Progress = 0;
					break;
			}

		}
	}

	private async Task AnimateProgress()
	{
		if (progress == null) return;

		progress.Progress = 0;

		await this.Dispatcher.DispatchAsync(async () =>
		{
			await progress.ProgressTo(1, 600, Easing.Linear);
		});

		// sync back to model
		if (BindingContext is StepBarModel model)
			model.ProgressValue = 1;
	}

	protected override void OnParentSet()
	{
		base.OnParentSet();

		// When view is removed from visual tree → cleanup
		if (Parent == null && _previousModel != null)
		{
			_previousModel.PropertyChanged -= Model_PropertyChanged;
			_previousModel = null;
		}
	}

	private void StepTapped_Tapped(object sender, EventArgs e)
	{
		var model = ((View)sender).BindingContext as StepBarModel;
		if (model.Status == StepBarStatus.Completed)
		{
			model.Status = StepBarStatus.InProgress;
		}
		model.IsCurrentContent = false;
		StepSelected?.Invoke(model.MainContent);
		progress.Progress = 0;
	}
}
