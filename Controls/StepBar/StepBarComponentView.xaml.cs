using AirIQ.Controls.StepBar;
using AirIQ.ViewModels.Common;

namespace AirIQ.Controls;

public partial class StepBarComponentView : ContentView
{
	public StepBarComponentView()
	{
		InitializeComponent();

	}

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (BindingContext is ViewModelBase vm)
		{
			BindingContext = vm;

			collectionView.ItemTemplate = new DataTemplate(() =>
			{
				int count = collectionView.ItemsSource.Cast<object>().Count();
				var cell = new StepBarViewCell(count);

				// Pass step content dynamically (entirely within the control)
				cell.StepSelected += OnStepSelected;

				// Bind StepIndex to model
				//cell.SetBinding(StepBarViewCell.StepIndexProperty, "Index");

				return cell;
			});

			//collectionView.ItemTemplate = new DataTemplate(() => new StepBarViewCell(vm));
		}
	}

	private void OnStepSelected(View view)
	{
		MainDynamicContent.Content = view;
	}
}