using AirIQ.Views;
using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class BankDetailsPage : BasePage
{
	public BankDetailsPage(BankDetailsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}