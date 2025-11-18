using AirIQ.Controls;
using System.Collections;
using System.ComponentModel;
using System.Windows.Input;

namespace AirIQ.Controls;

public partial class ExtendedDropdown : ContentView
{

    public ExtendedDropdown()
    {
        InitializeComponent();

        dropdown.SetBinding(CustomDropdown.ShowDropdownProperty,
                    new Binding(nameof(OnLoadShowDropdown), BindingMode.TwoWay, source: this));
    }

    /// <summary>
    ///     Identifies the <see cref="Title" /> bindable property.
    /// </summary>
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ExtendedDropdown));

    /// <summary>
    ///     Identifies the <see cref="PlaceholderText" /> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceholderTextProperty =
        BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(ExtendedDropdown));

    /// <summary>
    ///     Identifies the <see cref="ItemsSource" /> bindable property.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(ExtendedDropdown),
            default(IList), BindingMode.TwoWay);

    /// <summary>
    ///     Identifies the <see cref="SelectedItem" /> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(ExtendedDropdown), null,
            BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

    /// <summary>
    ///     Identifies the <see cref="ErrorText" /> bindable property.
    /// </summary>
    public static readonly BindableProperty ErrorTextProperty =
        BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(ExtendedDropdown),
            default(string), BindingMode.TwoWay);

    /// <summary>
    ///     Identifies the <see cref="DisplayMemberPath" /> bindable property.
    /// </summary>
    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(ExtendedDropdown));

    /// <summary>
    ///     Identifies the <see cref="CanEdit" /> bindable property.
    /// </summary>
    public static readonly BindableProperty CanEditProperty =
        BindableProperty.Create(nameof(CanEdit), typeof(bool), typeof(ExtendedDropdown), false);

    /// <summary>
    ///     Identifies the <see cref="OnLoadShowDropdown" /> bindable property.
    /// </summary>
    public static readonly BindableProperty OnLoadShowDropdownProperty =
        BindableProperty.Create(nameof(OnLoadShowDropdown), typeof(bool), typeof(ExtendedDropdown),
            false, BindingMode.TwoWay);

    /// <summary>
    ///     set icon for the control
    /// </summary>
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(ExtendedDropdown),
            "arrow_down.png", BindingMode.TwoWay);

    /// <summary>
    ///    Identifies the <see cref="IsError" /> bindable property.
    /// </summary>
    public static readonly BindableProperty IsErrorProperty =
        BindableProperty.Create(nameof(IsError), typeof(bool), typeof(ExtendedDropdown), false,
            BindingMode.TwoWay);

    /// <summary>
    ///     set sub-title for the control
    /// </summary>
    public static readonly BindableProperty IsFontImagePoperty =
        BindableProperty.Create(nameof(IsFontImage), typeof(bool), typeof(ExtendedDropdown), true,
            BindingMode.TwoWay);

    public static readonly BindableProperty InputViewStyleProperty =
        BindableProperty.Create(nameof(InputViewStyle), typeof(Style), typeof(ExtendedDropdown));

        public static readonly BindableProperty TextChangeCommandProperty =
        BindableProperty.Create(nameof(TextChangeCommand), typeof(ICommand), typeof(ExtendedDropdown));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public bool IsError
    {
        get => (bool)GetValue(IsErrorProperty);
        set => SetValue(IsErrorProperty, value);
    }

    public bool IsFontImage
    {
        get => (bool)GetValue(IsFontImagePoperty);
        set => SetValue(IsFontImagePoperty, value);
    }

    public Style InputViewStyle
    {
        get => (Style)GetValue(InputViewStyleProperty);
        set => SetValue(InputViewStyleProperty, value);
    }

    [TypeConverter(typeof(ImageSource))]
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public BindingBase ItemDisplayBinding { get; set; }
    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    public bool CanEdit
    {
        get => (bool)GetValue(CanEditProperty);
        set => SetValue(CanEditProperty, value);
    }

    public bool OnLoadShowDropdown
    {
        get => (bool)GetValue(OnLoadShowDropdownProperty);
        set => SetValue(OnLoadShowDropdownProperty, value);
    }

    public ICommand TextChangeCommand
    {
        get => (ICommand)GetValue(TextChangeCommandProperty);
        set => SetValue(TextChangeCommandProperty, value);
    }

    #region Events

   

    public event EventHandler SelectedIndexChanged;

    #endregion

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as ExtendedDropdown;
        if (newValue != null)
        {
            string selectedText = string.Empty;
            if (string.IsNullOrEmpty(control.DisplayMemberPath))
            {
                selectedText = newValue.ToString();
            }
            else
            {
                selectedText = newValue?.GetType().GetProperty(control.DisplayMemberPath)?.GetValue(newValue)?.ToString() ?? "";
            }
        }
    }

    void LifecycleEffectLoaded(object sender, EventArgs e)
    {
        dropdown.ShowDropdown = OnLoadShowDropdown;
    }

    void TapGestureRecognizerTapped(object sender, EventArgs e)
    {
        dropdown.ShowDropdown = !dropdown.ShowDropdown;
        Icon = dropdown.ShowDropdown ? "arrow_up.png" : "arrow_down.png";
    }

    private void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedIndexChanged?.Invoke(sender, e);
    }

    private void DropdownTextChanged(object sender, string e)
    {
        TextChangeCommand?.Execute(e);
    }
}