using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class DashboardPage : BasePage
{
	public DashboardPage(DashboardPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}