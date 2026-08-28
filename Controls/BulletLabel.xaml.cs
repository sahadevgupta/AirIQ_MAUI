using Microsoft.Maui.Controls;

namespace AirIQ.Controls;

public partial class BulletLabel : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(BulletLabel), string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public BulletLabel()
    {
        InitializeComponent();
    }
}