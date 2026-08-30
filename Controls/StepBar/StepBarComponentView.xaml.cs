using System.ComponentModel;
using AirIQ.Controls.StepBar;
using AirIQ.ViewModels;
using AirIQ.ViewModels.Common;

namespace AirIQ.Controls;

public partial class StepBarComponentView : ContentView
{
	private SignupPageViewModel? _observedVm;

	public StepBarComponentView()
	{
		InitializeComponent();
		SizeChanged += OnSizeChanged;
	}

	private void OnSizeChanged(object? sender, EventArgs e) => UpdateStepWidths();

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		if (BindingContext is ViewModelBase vm)
		{
			if (collectionView.ItemTemplate == null)
			{
				collectionView.ItemTemplate = new DataTemplate(() =>
				{
					var cell = new StepBarViewCell();

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

				// Steps starts empty and is only populated later by LoadDataWhenNavigatedTo,
				// after this control's initial size/binding events have already fired, so we
				// also need to react once the Steps collection itself is (re)assigned.
				if (!ReferenceEquals(_observedVm, signupVm))
				{
					_observedVm?.PropertyChanged -= OnSignupVmPropertyChanged;
					_observedVm = signupVm;
					_observedVm.PropertyChanged += OnSignupVmPropertyChanged;
				}
			}

			UpdateStepWidths();
		}
	}

	private void OnSignupVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(SignupPageViewModel.Steps))
		{
			UpdateStepWidths();
		}
	}

	// Cell width is driven by this control's own rendered width (not the raw device
	// screen width), so the progress bars stay correctly sized on tablets and split-screen
	// layouts, and only recompute when the control's size, or the Steps collection, actually changes.
	private void UpdateStepWidths()
	{
		if (Width <= 0 || BindingContext is not SignupPageViewModel signupVm || signupVm.Steps.Count == 0)
		{
			return;
		}

		double cellWidth = Width / signupVm.Steps.Count;
		foreach (var step in signupVm.Steps)
		{
			step.ListWidth = cellWidth;
		}
	}

	private void OnStepSelected(View view)
	{
		MainDynamicContent.Content = view;
	}
}