using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class OnlineRechargePage : BasePage
{
	public OnlineRechargePage(OnlineRechargePageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}