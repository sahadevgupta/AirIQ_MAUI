using System.Windows.Input;

namespace AirIQ.Controls;

public partial class SwitchControl : ContentView
{
    public static readonly BindableProperty IsToggledProperty =
        BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(SwitchControl), false, BindingMode.TwoWay, propertyChanged: OnIsToggledChanged);

    public static readonly BindableProperty ToggledCommandProperty =
        BindableProperty.Create(nameof(ToggledCommand), typeof(ICommand), typeof(SwitchControl), default(ICommand));

    public event EventHandler<bool> Toggled;

    public bool IsToggled
    {
        get => (bool)GetValue(IsToggledProperty);
        set => SetValue(IsToggledProperty, value);
    }

    public ICommand ToggledCommand
    {
        get => (ICommand)GetValue(ToggledCommandProperty);
        set => SetValue(ToggledCommandProperty, value);
    }

    public SwitchControl()
    {
        InitializeComponent();
        UpdateImage();
    }

    private void OnToggleTapped(object sender, TappedEventArgs e)
    {
        IsToggled = !IsToggled;
    }

    private static void OnIsToggledChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SwitchControl control) return;

        control.UpdateImage();
        control.Toggled?.Invoke(control, control.IsToggled);

        if (control.ToggledCommand?.CanExecute(control.IsToggled) == true)
            control.ToggledCommand.Execute(control.IsToggled);
    }

    private void UpdateImage()
    {
        ToggleImage.Source = IsToggled ? "toggle_on" : "toggle_off";
    }
}
