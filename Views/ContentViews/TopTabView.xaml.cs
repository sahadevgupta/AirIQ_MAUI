using AirIQ.Enums;
using AirIQ.Services.Interfaces;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AirIQ.Views.ContentViews;

public partial class TopTabView : ContentView
{ 
    public double PreviousMeasuredWidth = -1; 
    public const double MinFontSize = 12; 
    public const double DefaultFontSize = 13.5; 
    public Label measureLabel;


    public static readonly BindableProperty SelectedTabProperty = BindableProperty.Create(nameof(SelectedTab),typeof(BookingTypes),typeof(TopTabView), BookingTypes.OneWay,BindingMode.TwoWay,propertyChanged: OnSelectedTabChanged);
    public static readonly BindableProperty TabChangedCommandProperty =BindableProperty.Create(nameof(TabChangedCommand), typeof(ICommand), typeof(BookingTypes));
   

    public BookingTypes SelectedTab
    {
        get => (BookingTypes)GetValue(SelectedTabProperty);
        set => SetValue(SelectedTabProperty, value);
    }

    public ICommand TabChangedCommand
    {
        get => (ICommand)GetValue(TabChangedCommandProperty);
        set => SetValue(TabChangedCommandProperty, value);
    }

    public TopTabView() 
    { 
        InitializeComponent();
        SetTemplateFor(SelectedTab);
    }

    private void OneWayClicked(object sender, TappedEventArgs e)
    {
        
        ChangeTab(BookingTypes.OneWay);
    }

    private void RoundTripClicked(object sender, TappedEventArgs e)
    {
        ChangeTab(BookingTypes.RoundTrip);
    }

    void ChangeTab(BookingTypes tab)
    {
        if (SelectedTab == tab) return;
        SelectedTab = tab; // triggers propertyChanged
        if (TabChangedCommand?.CanExecute(tab) == true)
            TabChangedCommand.Execute(tab);
    }

    static void OnSelectedTabChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is TopTabView view && newValue is BookingTypes tab)
            view.SetTemplateFor(tab);
    }

    void SetTemplateFor(BookingTypes tab)
    {
        try
        {
            DynamicHost.Content = null;
            if (!Resources.TryGetValue("BookingTypeSelector", out var selectorObj))
            {
                System.Diagnostics.Debug.WriteLine("Selector not found in Resources.");
                return;
            }

            if (selectorObj is not DataTemplateSelector selector)
            {
                System.Diagnostics.Debug.WriteLine("Resource 'BookingTypeSelector' is not a DataTemplateSelector.");
                return;
            }

            var template = selector.SelectTemplate(tab, this);

            if (template == null)
            {
                System.Diagnostics.Debug.WriteLine("Selector returned null template.");
                return;
            }

            if (template.CreateContent() is View view)
            {
                DynamicHost.Content = view;
                System.Diagnostics.Debug.WriteLine("Switched template to {tab}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Template.CreateContent() did not return a View.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Exception in SetTemplateFor: {ex}");
        }
    }
}