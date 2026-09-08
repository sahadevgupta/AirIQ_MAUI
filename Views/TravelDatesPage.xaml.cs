using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class TravelDatesPage : BasePage
{
    public TravelDatesPage(TravelDatesPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
