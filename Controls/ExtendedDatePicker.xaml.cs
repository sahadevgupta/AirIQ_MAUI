namespace AirIQ.Controls;

public partial class ExtendedDatePicker : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ExtendedDatePicker), null);

    public static readonly BindableProperty TitleStyleProperty =
       BindableProperty.Create(propertyName: nameof(TitleStyle), returnType: typeof(Style), declaringType: typeof(ExtendedDatePicker), (Style)Application.Current.Resources["TitleSmallWithPrimaryFontColor"], defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty DateProperty =
       BindableProperty.Create(propertyName: nameof(Date), returnType: typeof(DateTime), declaringType: typeof(ExtendedDatePicker), default(DateTime), defaultBindingMode: BindingMode.TwoWay);


    public Style TitleStyle
    {
        get { return (Style)GetValue(TitleStyleProperty); }
        set { SetValue(TitleStyleProperty, value); }
    }

    public string Title
    {
        get { return (string)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public DateTime Date
    {
        get { return (DateTime)GetValue(DateProperty); }
        set { SetValue(DateProperty, value); }
    }
    public ExtendedDatePicker()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
        datepicker.Focus();
    }

    private void datepicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        datelbl.Text = e.NewDate.ToString("dd/MM/yyyy");
        datelbl.TextColor = Colors.Black;
    }
}
