using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class SalesRecordPage : BasePage
{
	public SalesRecordPage(SalesRecordPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}