using AirIQ.ViewModels.Common;

namespace AirIQ.Views;

public partial class ForgotPasswordPage : BasePage
{
	public ForgotPasswordPage(ForgotPasswordPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}