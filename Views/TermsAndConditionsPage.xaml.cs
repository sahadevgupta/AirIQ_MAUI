using AirIQ.ViewModels;
using AirIQ.Views;
using Microsoft.Maui.Controls;

namespace AirIQ.Views;

public partial class TermsAndConditionsPage : BasePage
{
    public TermsAndConditionsPage(LegalViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}