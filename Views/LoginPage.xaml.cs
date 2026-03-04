using AirIQ.ViewModels;
namespace AirIQ.Views;

public partial class LoginPage : BasePage
{
	public LoginPage(LoginPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}