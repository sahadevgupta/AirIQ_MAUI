using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class SignupPage : BasePage
{
	public SignupPage(SignupPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}