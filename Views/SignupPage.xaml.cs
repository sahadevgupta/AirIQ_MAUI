using AirIQ.Helpers;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class SignupPage : BasePage
{
	readonly SignupPageViewModel _viewModel;
	public SignupPage(SignupPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;

		//InitializeView();
	}

	private async void InitializeView()
	{
		var _dialogService = ServiceHelper.GetService<ILoadingPopUpService>();

		_dialogService!.Show();
		await Task.Delay(100);

		_dialogService.Hide();
	}
}