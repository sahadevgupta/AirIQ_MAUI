using Mopups.Pages;
using Mopups.Services;

namespace AirIQ.Popups;

public partial class TravelersPopup : PopupPage
{
    public static readonly BindableProperty AdultCountProperty =
        BindableProperty.Create(nameof(AdultCount), typeof(int), typeof(TravelersPopup), 1, BindingMode.TwoWay, propertyChanged: OnCountChanged);

    public static readonly BindableProperty ChildCountProperty =
        BindableProperty.Create(nameof(ChildCount), typeof(int), typeof(TravelersPopup), 0, BindingMode.TwoWay, propertyChanged: OnCountChanged);

    public static readonly BindableProperty InfantCountProperty =
        BindableProperty.Create(nameof(InfantCount), typeof(int), typeof(TravelersPopup), 0, BindingMode.TwoWay, propertyChanged: OnCountChanged);

    static readonly BindablePropertyKey TotalTravelersPropertyKey =
        BindableProperty.CreateReadOnly(nameof(TotalTravelers), typeof(int), typeof(TravelersPopup), 1);

    public static readonly BindableProperty TotalTravelersProperty = TotalTravelersPropertyKey.BindableProperty;

    public int AdultCount
    {
        get => (int)GetValue(AdultCountProperty);
        set => SetValue(AdultCountProperty, value);
    }

    public int ChildCount
    {
        get => (int)GetValue(ChildCountProperty);
        set => SetValue(ChildCountProperty, value);
    }

    public int InfantCount
    {
        get => (int)GetValue(InfantCountProperty);
        set => SetValue(InfantCountProperty, value);
    }

    public int TotalTravelers => (int)GetValue(TotalTravelersProperty);

    public event Action<int, int, int>? Confirmed;

    public TravelersPopup()
    {
        InitializeComponent();
    }

    static void OnCountChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var popup = (TravelersPopup)bindable;
        popup.SetValue(TotalTravelersPropertyKey, popup.AdultCount + popup.ChildCount + popup.InfantCount);
    }

    async void DoneClicked(object sender, EventArgs e)
    {
        Confirmed?.Invoke(AdultCount, ChildCount, InfantCount);
        await MopupService.Instance.PopAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Confirmed = null;
    }
}
