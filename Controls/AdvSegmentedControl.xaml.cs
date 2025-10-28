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
        BindableProperty.Create(nameof(ItemSelected), typeof(object), typeof(AdvSegmentedControl), null, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => OnItemSelectedChanged(bindable, oldValue, newValue));



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
        BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(Command<object>), typeof(AdvSegmentedControl), default(Command<object>), BindingMode.TwoWay, null, SelectedItemChangedCommandPropertyChanged);



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

    public Command<string> SelectedItemChangedCommand
    {
        get
        {
            return (Command<string>)GetValue(SelectedItemChangedCommandProperty);
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

    public ICommand TapCommand => new Command(OnTap);
    void OnTap(object val)
    {
        ItemSelected = val;
    }

    private static void OnItemSelectedChanged(BindableObject bindable, object oldValue, object newValue)
    {

    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue != null)
        {
            // handle new items
            var control = (AdvSegmentedControl)bindable;
            control.TabButtonHolder.Children?.Clear();

            int index = 0;
            foreach (var item in (IEnumerable)newValue)
            {
                var newTab = new TabButton(
                    FormatType(item, control.DisplayMemberPath),
                    index,
                    ((AdvSegmentedControl)bindable).PrimaryColor,
                    ((AdvSegmentedControl)bindable).SecondaryColor,
                    index == 0 ? true : false,
                    item);

                newTab.TabButtonClicked += (sender, args) =>
                {
                    ((AdvSegmentedControl)bindable).SelectedTabIndex = ((TabButton)sender).TabIndex;
                    ((AdvSegmentedControl)bindable).SendSelectedTabIndexChangedEvent();
                    ((AdvSegmentedControl)bindable).SelectedItemChangedCommand?.Execute(newValue);
                    foreach (var tabButton in ((AdvSegmentedControl)bindable).TabButtonHolder.Children)
                    {

                        var prop = tabButton.GetType().GetRuntimeProperties().FirstOrDefault(p => string.Equals(p.Name, control.SelectionIndicator, StringComparison.OrdinalIgnoreCase));
                        if (prop != null)
                        {
                            prop.SetValue(tabButton, tabButton.Equals(newValue));
                        }

                       ((TabButton)tabButton).UpdateTabButtonState(((AdvSegmentedControl)bindable).SelectedTabIndex);
                    }

                };


                Grid.SetColumn(newTab, index++);

                ((AdvSegmentedControl)bindable).TabButtonHolder.Children.Add(newTab);
            }

            if (((AdvSegmentedControl)bindable).SelectedTabIndex >
                ((AdvSegmentedControl)bindable).TabButtonHolder.Children.Count - 1)
            {
                ((AdvSegmentedControl)bindable).SelectedTabIndex = 0;
            }
        }
        else
        {
            ((AdvSegmentedControl)bindable).TabButtonHolder.Children?.Clear();
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
}
public class SelectedTabIndexEventArgs : EventArgs
{
    public int SelectedTabIndex { get; set; }
}
