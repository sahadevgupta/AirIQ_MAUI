using System.Diagnostics;

using AirIQ.ViewModels;

namespace AirIQ.Views;

public partial class TravelDatesPage : BasePage
{
    public TravelDatesPage(TravelDatesPageViewModel viewModel)
    {
        Debug.WriteLine("TravelDatesPage ctor start (DI resolved/constructing) : " + DateTime.Now);

        InitializeComponent();
        Debug.WriteLine("TravelDatesPage InitializeComponent done : " + DateTime.Now);

        BindingContext = viewModel;

        Debug.WriteLine("TravelDatesPage ctor end : " + DateTime.Now);
    }

    protected override void OnAppearing()
    {
        Debug.WriteLine("TravelDatesPage OnAppearing start : " + DateTime.Now);
        base.OnAppearing();
        Debug.WriteLine("TravelDatesPage OnAppearing end : " + DateTime.Now);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        Debug.WriteLine("TravelDatesPage OnNavigatedTo start : " + DateTime.Now);
        base.OnNavigatedTo(args);
        Debug.WriteLine("TravelDatesPage OnNavigatedTo end : " + DateTime.Now);
    }
}
