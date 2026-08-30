using AirIQ.ViewModels;
using AirIQ.Views;
using Microsoft.Maui.Controls;

namespace AirIQ.Views;

public partial class PrivacyPolicyPage : BasePage
{
    public PrivacyPolicyPage(LegalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}