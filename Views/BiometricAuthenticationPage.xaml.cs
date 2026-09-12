using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class BiometricAuthenticationPage : BasePage
{
    public BiometricAuthenticationPage(BiometricAuthenticationPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
