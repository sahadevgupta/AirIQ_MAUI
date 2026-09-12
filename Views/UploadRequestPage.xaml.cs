using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class UploadRequestPage : BasePage
{
	public UploadRequestPage(UploadRequestPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}