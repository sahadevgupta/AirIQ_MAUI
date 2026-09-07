using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class DepartureDatePage : BasePage
{
    public DepartureDatePage(DepartureDatePageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        calendar.DateSelected += (_, date) => viewModel.ConfirmSelectionCommand.Execute(date);
    }
}
