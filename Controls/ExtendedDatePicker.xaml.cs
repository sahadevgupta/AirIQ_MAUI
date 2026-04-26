using Microsoft.Maui.Handlers;
using Mopups.Interfaces;

namespace AirIQ.Controls;

public partial class ExtendedDatePicker : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ExtendedDatePicker), null);

    public static readonly BindableProperty TitleStyleProperty =
       BindableProperty.Create(propertyName: nameof(TitleStyle), returnType: typeof(Style), declaringType: typeof(ExtendedDatePicker), (Style)Application.Current.Resources["TitleSmallWithPrimaryFontColor"], defaultBindingMode: BindingMode.TwoWay);

    public static readonly BindableProperty DateProperty =
       BindableProperty.Create(propertyName: nameof(Date), returnType: typeof(DateTime), declaringType: typeof(ExtendedDatePicker), default(DateTime), defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
       {
           var control = (ExtendedDatePicker)bindable;
           if (newValue != null && (DateTime)newValue != default)
           {
               control.datelbl.Text = ((DateTime)newValue).ToString("dd/MM/yyyy");
               control.datelbl.TextColor = Colors.Black;
           }
       });

    public static readonly BindableProperty AllowedDatesProperty =
        BindableProperty.Create(nameof(AllowedDates), typeof(IList<DateTime>), typeof(ExtendedDatePicker), null, BindingMode.TwoWay);

    public IList<DateTime> AllowedDates
    {
        get => (IList<DateTime>)GetValue(AllowedDatesProperty);
        set => SetValue(AllowedDatesProperty, value);
    }

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
        set
        {
            SetValue(DateProperty, value);

        }
    }

    public event EventHandler? DateSelected;
    public ExtendedDatePicker()
    {
        InitializeComponent();
    }

    private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
    {
#if ANDROID
        var handler = datepicker.Handler as IDatePickerHandler;
        handler.PlatformView.PerformClick();
#else
        datepicker.Focus();
#endif
    }

    private void datepicker_Closed(object sender, DatePickerClosedEventArgs e)
    {
        if (datepicker.Date != null)
        {
            Date = datepicker.Date.Value;
            var selectedDate = datepicker.Date.Value.ToString("yyyy/MM/dd");
            datelbl.Text = selectedDate;
            DateSelected?.Invoke(this, e);
        }
    }

}
