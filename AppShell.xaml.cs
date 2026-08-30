using AirIQ.Configurations;
using AirIQ.Enums;
using AirIQ.Views;
using AirIQ_MAUI.Views;

namespace AirIQ;

public partial class AppShell : Shell
{
	// Maps the last route segment of a menu-linked page to its MenuType, so the popup's
	// selected item can be derived from Shell's actual navigation state rather than tracked separately.
	static readonly Dictionary<string, MenuType> RouteToMenuTypeMap = new()
	{
		["home"] = MenuType.Flight,
		[nameof(SalesRecordPage)] = MenuType.SalesRecord,
		[nameof(RefundsRecordPage)] = MenuType.RefundsRecord,
		[nameof(MyAccountPage)] = MenuType.Account,
		[nameof(AccountLedgerRecordPage)] = MenuType.AccountsLedger,
		[nameof(UploadRequestPage)] = MenuType.UploadRequest,
		[nameof(TempCreditPage)] = MenuType.TemporaryCredit,
		[nameof(BankDetailsPage)] = MenuType.BankDetails,
		[nameof(GroupQueryPage)] = MenuType.GroupQuery,
		[nameof(PaxCalendarPage)] = MenuType.PaxCalendar,
		[nameof(OnlineRechargePage)] = MenuType.OnlineRecharge,
	};

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
		Routing.RegisterRoute(nameof(MyAccountPage), typeof(MyAccountPage));

		Navigated += OnShellNavigated;
	}

	void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
	{
		var segment = e.Current?.Location.OriginalString
			.Split('/', StringSplitOptions.RemoveEmptyEntries)
			.LastOrDefault();

		if (segment is not null && RouteToMenuTypeMap.TryGetValue(segment, out var menuType))
			AppConfiguration.SelectedMenuType = menuType;
	}
}