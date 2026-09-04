using AirIQ.Controls;
using AirIQ.Helpers;
using AirIQ.Popups;
using AirIQ.ViewModels;
using Mopups.Interfaces;

namespace AirIQ.Views;

public partial class DashboardPage2 : BasePage
{
	public DashboardPage2(DashboardPage2ViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	void DepartureTapped(object sender, EventArgs e)
	{
		if (BindingContext is not DashboardPage2ViewModel viewModel)
			return;

		var popupNavigation = ServiceHelper.GetService<IPopupNavigation>();
		if (popupNavigation is null)
			return;

		var popup = new CalendarViewV2();
		popup.SetBinding(CalendarViewV2.AllowedDatesProperty,
			new Binding(nameof(DashboardPage2ViewModel.AllowedDates), source: viewModel));
		popup.DatePicked += date => viewModel.SelectedTravelDate = date;

		popupNavigation.PushAsync(popup);
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
}
