using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class TempCreditPage : BasePage
{
	public TempCreditPage(TempCreditPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}