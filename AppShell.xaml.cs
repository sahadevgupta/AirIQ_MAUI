using AirIQ.Views;
using AirIQ_MAUI.Views;

namespace AirIQ;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Apply custom TabBar styling
#if ANDROID
		AirIQ.Platforms.Handlers.TabBarCustomization.CustomizeTabBar(this);
#elif IOS
		AirIQ.Platforms.Handlers.TabBarIOSCustomization.CustomizeTabBar(this);
#endif

		Routing.RegisterRoute(nameof(ChangePasswordPage), typeof(ChangePasswordPage));
		Routing.RegisterRoute(nameof(FlightsPage), typeof(FlightsPage));
		Routing.RegisterRoute(nameof(FlightBookingPage), typeof(FlightBookingPage));
		Routing.RegisterRoute(nameof(ForgotPasswordPage), typeof(ForgotPasswordPage));
		Routing.RegisterRoute(nameof(SignupPage), typeof(SignupPage));
		Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));
		Routing.RegisterRoute(nameof(SalesRecordPage), typeof(SalesRecordPage));
		Routing.RegisterRoute(nameof(RefundsRecordPage), typeof(RefundsRecordPage));
		Routing.RegisterRoute(nameof(AccountLedgerRecordPage), typeof(AccountLedgerRecordPage));
		Routing.RegisterRoute(nameof(TempCreditPage), typeof(TempCreditPage));
		Routing.RegisterRoute(nameof(BankDetailsPage), typeof(BankDetailsPage));
		Routing.RegisterRoute(nameof(GroupQueryPage), typeof(GroupQueryPage));
		Routing.RegisterRoute(nameof(UploadRequestPage), typeof(UploadRequestPage));
		Routing.RegisterRoute(nameof(OnlineRechargePage), typeof(OnlineRechargePage));
		Routing.RegisterRoute(nameof(PaxCalendarPage), typeof(PaxCalendarPage));
		Routing.RegisterRoute(nameof(TermsAndConditionsPage), typeof(TermsAndConditionsPage));
		Routing.RegisterRoute(nameof(PrivacyPolicyPage), typeof(PrivacyPolicyPage));
	}
}