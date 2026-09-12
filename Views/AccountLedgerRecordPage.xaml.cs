using AirIQ.ViewModels;
using AirIQ.Views;

namespace AirIQ.Views;

public partial class AccountLedgerRecordPage : BasePage
{
	public AccountLedgerRecordPage(AccountLedgerRecordPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}