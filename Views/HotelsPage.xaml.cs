using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class HotelsPage : BasePage
{
	public HotelsPage(HotelsPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}