using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class GroupQueryPage : BasePage
{
	public GroupQueryPage(GroupQueryPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}