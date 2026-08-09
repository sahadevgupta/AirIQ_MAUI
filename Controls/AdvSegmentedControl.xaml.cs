using System.Collections;
using System.Reflection;
using System.Windows.Input;

namespace AirIQ.Controls;

public partial class AdvSegmentedControl : ContentView
{

    /// <summary>
    /// Identifies the <see cref="PrimaryColorProperty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty PrimaryColorProperty =
        BindableProperty.Create(nameof(PrimaryColor), typeof(Color), typeof(AdvSegmentedControl), Colors.CornflowerBlue, BindingMode.TwoWay);

    /// <summary>
    /// Identifies the <see cref="SecondaryColorProperty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty SecondaryColorProperty =
        BindableProperty.Create(nameof(SecondaryColor), typeof(Color), typeof(AdvSegmentedControl), Colors.White, BindingMode.TwoWay);

    /// <summary>
    /// Identifies the <see cref="DisplayMemberPath"/> bindable property.
    /// </summary>
    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(AdvSegmentedControl), string.Empty, BindingMode.OneWay, null, null);

    /// <summary>
    /// Identifies the <see cref="ItemSelectedProperty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ItemSelectedProperty =
        BindableProperty.Create(nameof(ItemSelected), typeof(object), typeof(AdvSegmentedControl), null, BindingMode.TwoWay);



    /// <summary>
    /// Identifies the <see cref="ItemsSourceProperty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AdvSegmentedControl), new List<object>(), propertyChanged: (bindable, oldValue, newValue) => OnItemsSourceChanged(bindable, oldValue, newValue));

    /// <summary>
    /// Identifies the <see cref="SelectionIndicatorProperty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectionIndicatorProperty =
        BindableProperty.Create(nameof(SelectionIndicator), typeof(string), typeof(AdvSegmentedControl), string.Empty);

    /// <summary>
    /// Identifies the <see cref="SelectedItemChangedCommandProperty"/> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectedItemChangedCommandProperty =
        BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(AdvSegmentedControl), default(ICommand), BindingMode.TwoWay);

    public static readonly BindableProperty ItemTemplateProperty
                = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AdvSegmentedControl), default(DataTemplate));

    public static readonly BindableProperty ItemSpacingProperty
        = BindableProperty.Create(nameof(ItemSpacing), typeof(double), typeof(AdvSegmentedControl), 0.0);

    public DataTemplate ItemTemplate
    {
        get { return (DataTemplate)GetValue(ItemTemplateProperty); }
        set { SetValue(ItemTemplateProperty, value); }
    }

    public double ItemSpacing
    {
        get { return (double)GetValue(ItemSpacingProperty); }
        set { SetValue(ItemSpacingProperty, value); }
    }

    public object ItemSelected
    {
        get
        {
            return (object)GetValue(ItemSelectedProperty);
        }
        set
        {
            SetValue(ItemSelectedProperty, value);

        }
    }

    public string SelectionIndicator
    {
        get
        {
            return (string)GetValue(SelectionIndicatorProperty);
        }
        set
        {
            SetValue(SelectionIndicatorProperty, value);

        }
    }

    static void SelectedItemChangedCommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var source = bindable as AdvSegmentedControl;
        if (source == null)
        {
            return;
        }
        source.SelectedItemChangedCommandChanged();
    }

    private void SelectedItemChangedCommandChanged()
    {
        OnPropertyChanged("SelectedItemChangedCommand");
    }

    public ICommand SelectedItemChangedCommand
    {
        get
        {
            return (ICommand)GetValue(SelectedItemChangedCommandProperty);
        }
        set
        {
            SetValue(SelectedItemChangedCommandProperty, value);
        }
    }

    /// <summary>
    /// Gets or sets the name or path of the property that is displayed for each data item.
    /// </summary>
    /// <value>
    /// The name or path of the property that is displayed for each the data item in
    /// the control. The default is an empty string ("").
    /// </value>
    public string DisplayMemberPath
    {
        get { return (string)GetValue(DisplayMemberPathProperty); }
        set { SetValue(DisplayMemberPathProperty, value); }
    }


    public IEnumerable ItemsSource
    {
        get
        {
            return (IEnumerable)GetValue(ItemsSourceProperty);
        }
        set
        {
            SetValue(ItemsSourceProperty, value);
        }
    }


    //public static readonly BindableProperty PrimaryColorProperty
    //    = BindableProperty.Create(
    //        nameof(PrimaryColor),
    //        typeof(Color),
    //        typeof(AdvSegmentedControl),
    //        Color.CornflowerBlue,
    //        propertyChanged: (bindable, value, newValue) =>
    //        {
    //            foreach (var tabButton in ((AdvSegmentedControl)bindable).TabButtonHolder.Children)
    //            {
    //                ((TabButton)tabButton).UpdateTabButtonColors(((Color)newValue),
    //                    ((AdvSegmentedControl)bindable).SecondaryColor);

    //                ((TabButton)tabButton).UpdateTabButtonState(
    //                    ((AdvSegmentedControl)bindable).SelectedTabIndex);
    //            }
    //        },
    //        defaultBindingMode: BindingMode.TwoWay);

    public Color PrimaryColor
    {
        get { return (Color)GetValue(PrimaryColorProperty); }
        set { SetValue(PrimaryColorProperty, value); }
    }


    //public static readonly BindableProperty SecondaryColorProperty
    //    = BindableProperty.Create(
    //        nameof(SecondaryColor),
    //        typeof(Color),
    //        typeof(AdvSegmentedControl),
    //        Color.White,
    //        propertyChanged: (bindable, value, newValue) =>
    //        {
    //            if (Device.RuntimePlatform == Device.iOS)
    //            {
    //                ((AdvSegmentedControl)bindable).FrameView.BorderColor = ((Color)newValue);
    //            }

    //            foreach (var tabButton in ((AdvSegmentedControl)bindable).TabButtonHolder.Children)
    //            {
    //                ((TabButton)tabButton).UpdateTabButtonColors(
    //                    ((AdvSegmentedControl)bindable).PrimaryColor, ((Color)newValue));

    //                ((TabButton)tabButton).UpdateTabButtonState(
    //                    ((AdvSegmentedControl)bindable).SelectedTabIndex);
    //            }
    //        },
    //        defaultBindingMode: BindingMode.TwoWay);

    public Color SecondaryColor
    {
        get { return (Color)GetValue(SecondaryColorProperty); }
        set { SetValue(SecondaryColorProperty, value); }
    }


    public static readonly BindableProperty SelectedTabIndexProperty
        = BindableProperty.Create(
            nameof(SelectedTabIndex),
            typeof(int),
            typeof(AdvSegmentedControl),
            default(int), BindingMode.TwoWay,
            propertyChanged: (bindable, value, newValue) =>
            {

            });

    public int SelectedTabIndex
    {
        get { return (int)GetValue(SelectedTabIndexProperty); }
        set { SetValue(SelectedTabIndexProperty, value); }
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        // handle new items
        var control = (AdvSegmentedControl)bindable;
        if (newValue != null)
        {

            control.TabButtonHolder.Children?.Clear();
            control.TabButtonHolder.ColumnDefinitions.Clear();

            var items = ((IEnumerable)newValue).Cast<object>().ToList();

            for (int i = 0; i < items.Count; i++)
            {
                control.TabButtonHolder.ColumnDefinitions.Add(
                    new ColumnDefinition { Width = GridLength.Star });
            }

            int index = 0;

            foreach (var item in (IEnumerable)newValue)
            {
                View tabView;

                if (control.ItemTemplate != null)
                {
                    tabView = (View)control.ItemTemplate.CreateContent();
                    tabView.BindingContext = item;

                    // var tapGesture = new TapGestureRecognizer();
                    // tapGesture.Tapped += (_, __) =>
                    // {
                    //     control.SelectedTabIndex = index;
                    //     control.SendSelectedTabIndexChangedEvent();
                    // };

                    // tabView.GestureRecognizers.Add(tapGesture);
                }
                else
                {
                    tabView = new TabButton(
                        FormatType(item, control.DisplayMemberPath),
                        index,
                        control.PrimaryColor,
                        control.SecondaryColor,
                        //index == control.SelectedTabIndex,
                        index == 0 ? true : false,
                        item);
                }

                // var newTab = new TabButton(
                //     FormatType(item, control.DisplayMemberPath),
                //     index,
                //     control.PrimaryColor,
                //     control.SecondaryColor,
                //     index == 0 ? true : false,
                //     item);

                var tap = new TapGestureRecognizer();

                tap.Command = new Command(() =>
                {
                    var a = control.GetItemIndex(item);
                    control.SelectedTabIndex = a;
                    control.SendSelectedTabIndexChangedEvent();
                    control.SelectedItemChangedCommand?.Execute(item);
                    foreach (var tabButton in control.TabButtonHolder.Children)
                    {

                        var prop = tabButton.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, control.SelectionIndicator, StringComparison.OrdinalIgnoreCase));
                        if (prop != null)
                        {
                            prop.SetValue(tabButton, tabButton.Equals(newValue));
                        }
                    }
                });

                tabView.GestureRecognizers.Add(tap);

                // tabView.TabButtonClicked += (sender, args) =>
                // {
                //     control.SelectedTabIndex = ((TabButton)sender).TabIndex;
                //     control.SendSelectedTabIndexChangedEvent();
                //     control.SelectedItemChangedCommand?.Execute(newValue);
                //     foreach (var tabButton in control.TabButtonHolder.Children)
                //     {

                //         var prop = tabButton.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, control.SelectionIndicator, StringComparison.OrdinalIgnoreCase));
                //         if (prop != null)
                //         {
                //             prop.SetValue(tabButton, tabButton.Equals(newValue));
                //         }

                //         ((TabButton)tabButton).UpdateTabButtonState(((AdvSegmentedControl)bindable).SelectedTabIndex);
                //     }

                // };
                Grid.SetColumn(tabView, index);
                control.TabButtonHolder.Children.Add(tabView);

                index++;
            }

            if (control.SelectedTabIndex >
                control.TabButtonHolder.Children.Count - 1)
            {
                control.SelectedTabIndex = 0;
            }
        }
        else
        {
            control.TabButtonHolder.Children?.Clear();
        }
    }

    private static string FormatType(object instance, string memberPath)
    {
        if (!string.IsNullOrEmpty(memberPath))
            return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
        else
            return instance?.ToString() ?? "";
    }

    public AdvSegmentedControl()
    {
        InitializeComponent();

        //if (Device.RuntimePlatform == Device.iOS)
        {
            FrameView.Stroke = SecondaryColor;
        }
    }

    public event EventHandler<SelectedTabIndexEventArgs> SelectedTabIndexChanged;

    /// <summary>
    /// Invoke the SelectedTabIndexChanged event
    /// for whoever has subscribed so they can
    /// use it for any reative action
    /// </summary>
    private void SendSelectedTabIndexChangedEvent()
    {
        var eventArgs = new SelectedTabIndexEventArgs();
        eventArgs.SelectedTabIndex = SelectedTabIndex;

        SelectedTabIndexChanged?.Invoke(this, eventArgs);
    }

    private int GetItemIndex(object item)
    {
        if (ItemsSource == null)
            return -1;

        return ItemsSource
            .Cast<object>()
            .ToList()
            .IndexOf(item);
    }
}
public class SelectedTabIndexEventArgs : EventArgs
{
    public int SelectedTabIndex { get; set; }
}
