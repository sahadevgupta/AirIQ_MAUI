using System.Windows.Input;

namespace AirIQ.Controls;

public partial class ExtendedEntry : ContentView
{
    public ExtendedEntry()
    {
        InitializeComponent();
    }

    // Bindable Property for Title
    public static readonly BindableProperty TitleProperty =
                            BindableProperty.Create(nameof(Title),
                            typeof(string),
                            typeof(ExtendedEntry),
                            string.Empty);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    // Bindable Property for SuffixText
    public static readonly BindableProperty SuffixTextProperty =
                            BindableProperty.Create(nameof(SuffixText),
                            typeof(string),
                            typeof(ExtendedEntry),
                            string.Empty);

    public string SuffixText
    {
        get => (string)GetValue(SuffixTextProperty);
        set => SetValue(SuffixTextProperty, value);
    }

    // Bindable Property for Placeholder
    public static readonly BindableProperty PlaceholderTextProperty =
                            BindableProperty.Create(nameof(PlaceholderText),
                            typeof(string),
                            typeof(ExtendedEntry),
                            string.Empty);

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    // Bindable Property for Return Command
    public static readonly BindableProperty ReturnCommandProperty =
                            BindableProperty.Create(nameof(ReturnCommand),
                            typeof(ICommand),
                            typeof(ExtendedEntry),
                            default(ICommand));

    public ICommand ReturnCommand
    {
        get => (ICommand)GetValue(ReturnCommandProperty);
        set => SetValue(ReturnCommandProperty, value);
    }

    // Bindable Property for Icon
    public static readonly BindableProperty LeadingIconProperty =
                            BindableProperty.Create(nameof(LeadingIcon),
                            typeof(string),
                            typeof(ExtendedEntry),
                            null,
                            BindingMode.TwoWay);

    public string LeadingIcon
    {
        get => (string)GetValue(LeadingIconProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public static readonly BindableProperty TrailingIconProperty =
                            BindableProperty.Create(nameof(TrailingIcon),
                            typeof(string),
                            typeof(ExtendedEntry),
                            null,
                            BindingMode.TwoWay);

    public string TrailingIcon
    {
        get => (string)GetValue(TrailingIconProperty);
        set => SetValue(TrailingIconProperty, value);
    }

    public static readonly BindableProperty LeadingIconColorProperty =
                            BindableProperty.Create(nameof(LeadingIconColor),
                            typeof(Color),
                            typeof(ExtendedEntry));

    public Color LeadingIconColor
    {
        get => (Color)GetValue(LeadingIconColorProperty);
        set => SetValue(LeadingIconProperty, value);
    }

    public static readonly BindableProperty TrailingIconColorProperty =
                            BindableProperty.Create(nameof(TrailingIconColor),
                            typeof(Color),
                            typeof(ExtendedEntry));

    public Color TrailingIconColor
    {
        get => (Color)GetValue(TrailingIconColorProperty);
        set => SetValue(TrailingIconColorProperty, value);
    }

    public static readonly BindableProperty LeadingIconSizeProperty =
                            BindableProperty.Create(nameof(LeadingIconSize),
                            typeof(double),
                            typeof(ExtendedEntry),
                            default(double));

    public double LeadingIconSize
    {
        get => (double)GetValue(LeadingIconSizeProperty);
        set => SetValue(LeadingIconSizeProperty, value);
    }

    public static readonly BindableProperty TrailingIconSizeProperty =
                            BindableProperty.Create(nameof(TrailingIconSize),
                            typeof(double),
                            typeof(ExtendedEntry),
                             default(double));

    public double TrailingIconSize
    {
        get => (double)GetValue(TrailingIconSizeProperty);
        set => SetValue(TrailingIconSizeProperty, value);
    }


    // Bindable Property for Icon Tap Command
    public static readonly BindableProperty IconTapCommandProperty =
                            BindableProperty.Create(nameof(IconTapCommand),
                            typeof(ICommand),
                            typeof(ExtendedEntry),
                            default(ICommand));

    public ICommand IconTapCommand
    {
        get => (ICommand)GetValue(IconTapCommandProperty);
        set => SetValue(IconTapCommandProperty, value);
    }

    // Bindable Property for Icon Tap Command Parameter
    public static readonly BindableProperty IconTapCommandParameterProperty =
                            BindableProperty.Create(nameof(IconTapCommandParameter),
                            typeof(object),
                            typeof(ExtendedEntry),
                            null);

    public object IconTapCommandParameter
    {
        get => GetValue(IconTapCommandParameterProperty);
        set => SetValue(IconTapCommandParameterProperty, value);
    }

    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly),
        typeof(bool),
        typeof(ExtendedEntry),
        false);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    // Bindable Property for Entry Text
    public static readonly BindableProperty TextProperty =
                           BindableProperty.Create(nameof(Text),
                           typeof(string),
                           typeof(ExtendedEntry),
                           string.Empty,
                           BindingMode.TwoWay,
                           propertyChanged: OnEntryTextChanged);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static void OnEntryTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ExtendedEntry)bindable;
        if (control.InputEntry.Text != newValue?.ToString())
            control.InputEntry.Text = newValue?.ToString();
    }


    // property for keyboard type
    public static readonly BindableProperty EntryKeyboardProperty =
            BindableProperty.Create(nameof(EntryKeyboard), typeof(Keyboard), typeof(ExtendedEntry), Keyboard.Default);

    public Keyboard EntryKeyboard
    {
        get => (Keyboard)GetValue(EntryKeyboardProperty);
        set => SetValue(EntryKeyboardProperty, value);
    }

    //Bindable property for error view visibility
    public static readonly BindableProperty IsErrorViewVisibleProperty =
        BindableProperty.Create(nameof(IsErrorViewVisible),
        typeof(bool),
        typeof(ExtendedEntry),
        false);

    public bool IsErrorViewVisible
    {
        get => (bool)GetValue(IsErrorViewVisibleProperty);
        set => SetValue(IsErrorViewVisibleProperty, value);
    }

    //Bindable property for error text
    public static readonly BindableProperty ErrorTextProperty =
                           BindableProperty.Create(nameof(ErrorText),
                           typeof(string),
                           typeof(ExtendedEntry),
                           string.Empty);

    public string ErrorText
    {
        get => (string)GetValue(ErrorTextProperty);
        set => SetValue(ErrorTextProperty, value);
    }

    //Bindable property for supporting text
    public static readonly BindableProperty SupportingTextProperty =
                           BindableProperty.Create(nameof(SupportingText),
                           typeof(string),
                           typeof(ExtendedEntry),
                           string.Empty);

    public string SupportingText
    {
        get => (string)GetValue(SupportingTextProperty);
        set => SetValue(SupportingTextProperty, value);
    }

    // Bindable property for supporting text visibility
    public static readonly BindableProperty IsSupportingTextVisibleProperty =
        BindableProperty.Create(nameof(IsSupportingTextVisible),
        typeof(bool),
        typeof(ExtendedEntry),
        false);

    public bool IsSupportingTextVisible
    {
        get => (bool)GetValue(IsSupportingTextVisibleProperty);
        set => SetValue(IsSupportingTextVisibleProperty, value);
    }


    //Bindable property for entry border stroke color

    public static readonly BindableProperty EntryBorderColorProperty =
            BindableProperty.Create(
                nameof(EntryBorderColor),
                typeof(Color),
                typeof(ExtendedEntry),
                (Color)(Application.Current?.Resources["LightGray"])!,
                BindingMode.TwoWay,
                propertyChanged: OnEntryBorderColorPropertyChanged);
    public Color EntryBorderColor
    {
        get => (Color)GetValue(EntryBorderColorProperty);
        set => SetValue(EntryBorderColorProperty, value);
    }

    //Bindable property for entry max length
    public static readonly BindableProperty MaxLengthProperty =
            BindableProperty.Create(
                nameof(MaxLength),
                typeof(int),
                typeof(ExtendedEntry),
                int.MaxValue);
    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly BindableProperty LeadingIconTypeProperty =
    BindableProperty.Create(
        nameof(LeadingIconType),
        typeof(CustomIconType),
        typeof(ExtendedEntry),
        CustomIconType.None);

    public CustomIconType LeadingIconType
    {
        get => (CustomIconType)GetValue(LeadingIconTypeProperty);
        set => SetValue(LeadingIconTypeProperty, value);
    }


    public static readonly BindableProperty TrailingIconTypeProperty =
        BindableProperty.Create(
            nameof(TrailingIconType),
            typeof(CustomIconType),
            typeof(ExtendedEntry),
            CustomIconType.None);

    public CustomIconType TrailingIconType
    {
        get => (CustomIconType)GetValue(TrailingIconTypeProperty);
        set => SetValue(TrailingIconTypeProperty, value);
    }

    #region [ Show/Hide Password ]

    private bool _isPassword;
    public bool IsPassword
    {
        get => _isPassword;
        set
        {
            _isPassword = value;
            if (_isPassword)
            {
                TrailingIcon = "visibility";
                this.Dispatcher.Dispatch(async () =>
                {
                    await Task.Delay(50);
                    InputEntry.IsPassword = true;
                });

                IconTapCommand = new Command(() =>
                {
                    bool hasFocus = InputEntry.IsFocused;
                    if (InputEntry.IsPassword)
                    {
                        TrailingIcon = "visibility";
                        InputEntry.IsPassword = false;
                    }
                    else
                    {
                        TrailingIcon = "visibility";
                        InputEntry.IsPassword = true;
                    }

                    if (hasFocus)
                        InputEntry.Focus();
                });
            }
            else
            {
                InputEntry.Keyboard = this.EntryKeyboard;
            }
        }
    }

    #endregion

    public event EventHandler<TextChangedEventArgs> TextChanged;

    private static void OnEntryBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ExtendedEntry control && newValue is Color newColor)
        {
            control.EntryBorder.Stroke = newColor;
        }
    }

    private void InputEntry_Focused(object sender, FocusEventArgs e)
    {
        EntryBorderColor = (Application.Current?.Resources.TryGetValue("Black", out var value) == true
                     && value is Color color) ? color : Colors.Black;
    }

    void borderlessEntry_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        TextChanged?.Invoke(sender, e);
    }

}

