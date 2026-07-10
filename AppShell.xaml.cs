using AirIQ.Views;

namespace AirIQ;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
		Routing.RegisterRoute(nameof(FlightsPage), typeof(FlightsPage));
		Routing.RegisterRoute(nameof(FlightBookingPage), typeof(FlightBookingPage));
		Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
		Routing.RegisterRoute(nameof(SalesRecordPage), typeof(SalesRecordPage));
		Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
		Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));
	}
}