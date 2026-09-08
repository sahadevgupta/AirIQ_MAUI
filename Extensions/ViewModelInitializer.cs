using AirIQ.ViewModels;
using AirIQ.ViewModels.Common;

namespace AirIQ.Extensions
{
    public static class ViewModelInitializer
    {
        public static MauiAppBuilder ViewModelInit(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<ChangePasswordPageViewModel>()
                            .AddTransient<DashboardPageViewModel>()
                            .AddTransient<DashboardPage2ViewModel>()
                            .AddTransient<FlightsPageViewModel>()
                            .AddTransient<FlightBookingPageViewModel>()
                            .AddTransient<ForgotPasswordPageViewModel>()
                            .AddTransient<LoginPageViewModel>()
                            .AddTransient<MenuPageViewModel>()
                            .AddTransient<HotelsPageViewModel>()
                            .AddTransient<SalesRecordPageViewModel>()
                            .AddTransient<SignupPageViewModel>()
                            .AddTransient<SessionExpiryPopupViewModel>()
                            .AddTransient<RefundsRecordPageViewModel>()
                            .AddTransient<TempCreditPageViewModel>()
                            .AddTransient<GroupQueryPageViewModel>()
                            .AddTransient<AccountLedgerRecordPageViewModel>()
                            .AddTransient<UploadRequestPageViewModel>()
                            .AddTransient<OnlineRechargePageViewModel>()
                            .AddTransient<PaxCalendarPageViewModel>()
                            .AddTransient<BankDetailsPageViewModel>()
                            .AddTransient<MyAccountPageViewModel>()
                            .AddTransient<LegalViewModel>()
                            .AddTransient<AirportSearchPageViewModel>()
                            .AddTransient<DepartureDatePageViewModel>()
                            // Singleton: kept alive and pre-warmed by DashboardPage2ViewModel so
                            // opening the date picker only has to assign a few properties on an
                            // already-built calendar instead of constructing everything from scratch.
                            .AddSingleton<TravelDatesPageViewModel>()
                            .AddTransient<BiometricAuthenticationPageViewModel>()
                            .AddTransient<WalletPageViewModel>();

            return builder;
        }
    }
}
