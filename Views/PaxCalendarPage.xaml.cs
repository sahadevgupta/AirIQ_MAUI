using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class PaxCalendarPage : BasePage
{
	public PaxCalendarPage(PaxCalendarPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}