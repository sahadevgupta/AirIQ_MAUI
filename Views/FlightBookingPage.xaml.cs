using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class FlightBookingPage : BasePage
{
	public FlightBookingPage(FlightBookingPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}