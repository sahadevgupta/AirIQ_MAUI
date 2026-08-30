using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ_MAUI.Views;

public partial class GroupQueryPage : BasePage
{
	public GroupQueryPage(GroupQueryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}