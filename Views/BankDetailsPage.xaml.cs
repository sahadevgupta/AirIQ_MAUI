using AirIQ.Views;
using AirIQ.ViewModels;

namespace AirIQ_MAUI.Views;

public partial class BankDetailsPage : BasePage
{
	public BankDetailsPage(BankDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}