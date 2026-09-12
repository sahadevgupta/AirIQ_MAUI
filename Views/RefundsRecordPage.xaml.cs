using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class RefundsRecordPage : BasePage
{
	public RefundsRecordPage(RefundsRecordPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}