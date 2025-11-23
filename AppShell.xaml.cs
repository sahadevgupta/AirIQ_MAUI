using AirIQ.Views;

namespace AirIQ;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(FlightsPage), typeof(FlightsPage));
		Routing.RegisterRoute(nameof(FlightBookingPage), typeof(FlightBookingPage));
	}
}