using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class FlightsPage : BasePage
{
	public FlightsPage(FlightsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}