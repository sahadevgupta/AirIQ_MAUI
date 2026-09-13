using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class SettingsPage : BasePage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
