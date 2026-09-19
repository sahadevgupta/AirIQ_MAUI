using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class SummaryPage : BasePage
{
	public SummaryPage(SummaryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
