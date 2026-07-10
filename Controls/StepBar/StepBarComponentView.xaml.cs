using AirIQ.Controls.StepBar;
using AirIQ.ViewModels;
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
			if (collectionView.ItemTemplate == null)
			{
				collectionView.ItemTemplate = new DataTemplate(() =>
				{
					int count = 1;
					if (BindingContext is SignupPageViewModel signupViewModel)
					{
						count = Math.Max(1, signupViewModel.StepListCount);
					}

					var cell = new StepBarViewCell(count);

					// Pass step content dynamically (entirely within the control)
					cell.StepSelected += OnStepSelected;

					return cell;
				});
			}

			if (vm is SignupPageViewModel signupVm)
			{
				var currentStep = signupVm.Steps.FirstOrDefault(x => x.IsCurrentContent);
				if (currentStep?.MainContent != null)
				{
					MainDynamicContent.Content = currentStep.MainContent;
				}
			}

			//collectionView.ItemTemplate = new DataTemplate(() => new StepBarViewCell(vm));
		}
	}

	private void OnStepSelected(View view)
	{
		MainDynamicContent.Content = view;
	}
}