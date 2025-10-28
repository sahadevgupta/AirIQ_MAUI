namespace AirIQ.Controls;

public partial class TabButton : Grid
{
    public event EventHandler<EventArgs> TabButtonClicked;

    public string TabText { get; private set; }
    private int _tabIndex;

    public int TabIndex
    {
        get { return _tabIndex; }
        set { _tabIndex = value; }
    }



    public Color PrimaryColor { get; private set; }

    public Color SecondaryColor { get; private set; }



    public TabButton(string tabText, int tabIndex, Color primaryColor,
        Color secondaryColor, bool isSelectedByDefault, object bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
        // Set up default text values
        TabText = tabText;
        TabIndex = tabIndex;
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;

        TabLabelView.Text = TabText;

        // set up ios vertical separator 
        // visibility based on index
        //if (Device.RuntimePlatform == Device.iOS && TabIndex != 0)
        VerticalSeparator.IsVisible = true;

        if (Device.RuntimePlatform == Device.UWP)
            TabLabelView.Opacity = 0.35;

        // Set up default color values
        SetUpColorScheme();

        // set up selected status
        if (isSelectedByDefault)
        {
            TabButton_OnClicked(null, null);
        }
        else
        {
            SetUnselectedTabState();
        }

    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        var a = this.BindingContext;
    }

    private void SetUpColorScheme()
    {
        // Setting up platform specific properties for Android and iOS
        //if (Device.RuntimePlatform == Device.Android)
        //{
        //    TabLabelView.FontSize
        //        = Device.GetNamedSize(NamedSize.Medium, TabLabelView);

        //    TabButtonView.BackgroundColor = PrimaryColor;

        //    HorizontalIndicator.Color =
        //        TabLabelView.TextColor = SecondaryColor;
        //}
        //else if (Device.RuntimePlatform == Device.iOS)
        {
            TabLabelView.FontSize
                = Device.GetNamedSize(NamedSize.Small, TabLabelView);

            tabView.BackgroundColor = PrimaryColor;
            TabLabelView.TextColor = SecondaryColor;

            VerticalSeparator.Color = SecondaryColor;
        }
        //else if (Device.RuntimePlatform == Device.UWP)
        //{
        //    TabLabelView.FontSize
        //        = Device.GetNamedSize(NamedSize.Small, TabLabelView);

        //    TabButtonView.BackgroundColor = PrimaryColor;
        //    TabLabelView.TextColor = SecondaryColor;
        //}
    }

    private void TabButton_OnClicked(object sender, EventArgs e)
    {
        //SetSelectedTabState();

        SendTabButtonClicked();
    }

    private void SetSelectedTabState()
    {
        var dataContext = BindingContext;
        // set up platform specific
        // properties for SelectTab event
        //if (Device.RuntimePlatform == Device.Android)
        //{
        //    HorizontalIndicator.IsVisible = true;
        //}
        //else if (Device.RuntimePlatform == Device.iOS)
        {
            tabView.BackgroundColor = PrimaryColor;
            TabLabelView.TextColor = SecondaryColor;
        }
        //else if (Device.RuntimePlatform == Device.UWP)
        //{
        //    TabLabelView.Opacity = 1;
        //}
    }

    private void SetUnselectedTabState()
    {
        var dataContext = BindingContext;
        // set up platform specific
        // properties for UnselectTab event
        //if (Device.RuntimePlatform == Device.Android)
        //{
        //    //HorizontalIndicator.IsVisible = false;
        //}
        //else if (Device.RuntimePlatform == Device.iOS)
        {
            tabView.BackgroundColor = SecondaryColor;
            TabLabelView.TextColor = PrimaryColor;
        }
        //else if (Device.RuntimePlatform == Device.UWP)
        //{
        //    TabLabelView.Opacity = 0.35;
        //}
    }

    private void SendTabButtonClicked()
    {
        var dataContext = BindingContext;
        TabButtonClicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Update the Tab Button status Selected/Unselected
    /// </summary>
    /// <param name="selectedTabIndex"></param>
    public void UpdateTabButtonState(int selectedTabIndex)
    {
        if (selectedTabIndex != TabIndex)
            SetUnselectedTabState();
        else
            SetSelectedTabState();
    }

    /// <summary>
    /// Update the Color status of the Tab Button
    /// </summary>
    /// <param name="primaryColor"></param>
    /// <param name="secondaryColor"></param>
    public void UpdateTabButtonColors(Color primaryColor, Color secondaryColor)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;

        SetUpColorScheme();
    }
}
