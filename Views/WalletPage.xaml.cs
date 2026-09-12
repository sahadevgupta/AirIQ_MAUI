using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class WalletPage : BasePage
{
	public WalletPage(WalletPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
