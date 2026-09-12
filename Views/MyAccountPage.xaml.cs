using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class MyAccountPage : BasePage
{
	public MyAccountPage(MyAccountPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
