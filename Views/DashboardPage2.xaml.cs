using AirIQ.Helpers;
using AirIQ.Popups;
using AirIQ.Resources.Strings;
using AirIQ.ViewModels;
using Mopups.Interfaces;

namespace AirIQ.Views;

public partial class DashboardPage2 : BasePage
{
	private bool _isSwapAnimationRunning;

	public DashboardPage2(DashboardPage2ViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	void TravelersTapped(object sender, EventArgs e)
	{
		if (BindingContext is not DashboardPage2ViewModel viewModel)
			return;

		var popupNavigation = ServiceHelper.GetService<IPopupNavigation>();
		if (popupNavigation is null)
			return;

		var popup = new TravelersPopup
		{
			AdultCount = viewModel.AdultCount,
			ChildCount = viewModel.ChildCount,
			InfantCount = viewModel.InfantCount
		};

		popup.Confirmed += (adults, children, infants) =>
		{
			viewModel.AdultCount = adults;
			viewModel.ChildCount = children;
			viewModel.InfantCount = infants;
		};

		popupNavigation.PushAsync(popup);
	}

	async void SwapButtonClicked(object sender, EventArgs e)
	{
		// Guards against rapid repeated taps starting overlapping animations/swaps - a second tap
		// while one is already in flight is simply ignored rather than queued.
		if (_isSwapAnimationRunning)
			return;

		if (BindingContext is not DashboardPage2ViewModel viewModel)
			return;

		// Validated up front, before anything is touched: if the reverse route isn't available the
		// From/To values must stay exactly as they were, with no partial swap and no animation.
		if (!viewModel.IsReverseRouteAvailable())
		{
			await viewModel.ShowAlertAsync(AppResource.ReverseRouteNotAvailable);
			return;
		}

		_isSwapAnimationRunning = true;
		try
		{
			// Toggling between 0/180 (rather than animating back to 0 every time) avoids an extra
			// snap-back animation and gives continuous, alternating flip feedback on each tap.
			var targetRotation = swapIconBorder.Rotation == 0 ? 180 : 0;
			var rotateTask = swapIconBorder.RotateToAsync(targetRotation, 250, Easing.CubicInOut);

			// Fade the two detail blocks out first so the underlying data swap (instantaneous)
			// happens while they're invisible, hiding the jump instead of showing a value flicker.
			await Task.WhenAll(
				fromDetailsStack.FadeToAsync(0, 120, Easing.CubicIn),
				toDetailsStack.FadeToAsync(0, 120, Easing.CubicIn));

			viewModel.SwapSourceDestinationCommand.Execute(null);

			await Task.WhenAll(
				fromDetailsStack.FadeToAsync(1, 150, Easing.CubicOut),
				toDetailsStack.FadeToAsync(1, 150, Easing.CubicOut));

			await rotateTask;
		}
		finally
		{
			_isSwapAnimationRunning = false;
		}
	}
}
