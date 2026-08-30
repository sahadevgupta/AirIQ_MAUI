using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ_MAUI.Views;

public partial class MyAccountPage : BasePage
{
	public MyAccountPage(MyAccountPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
