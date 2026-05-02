using AirIQ.ViewModels.Common;

namespace AirIQ.Views;

public partial class ChangePasswordPage : BasePage
{
	public ChangePasswordPage(ChangePasswordPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}