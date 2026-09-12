using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class AirportSearchPage : BasePage
{
    public AirportSearchPage(AirportSearchPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(250), () => searchView.FocusSearchEntry());
    }
}
