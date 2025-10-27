using System.Collections;

namespace AirIQ.Controls;

public class CustomDropdown : View
{
    public void OnTextChanged(object arg)
    {
        TextChanged?.Invoke(this, arg.ToString());
    }

    public void OnItemSelected(object selectedItem)
    {
        UpdateSelectedIndex(selectedItem);
    }

    void UpdateSelectedIndex(object selectedItem)
    {
        if (ItemSource != null)
        {
            var index = ItemSource.IndexOf(selectedItem);
            if (index >= 0)
            {
                SelectedItem = ItemSource[index];
                ItemSelected?.Invoke(this, new ItemSelectedEventArgs { SelectedIndex = index });
            }
        }
    }

    #region Properties

    /// <summary>
    ///     Identifies the <see cref="ItemSource" /> bindable property.
    /// </summary>
    public static readonly BindableProperty ItemSourceProperty =
        BindableProperty.Create(nameof(ItemSource), typeof(IList), typeof(CustomDropdown),
            default(IList), BindingMode.TwoWay);

    /// <summary>
    ///     Identifies the <see cref="PlaceholderText" /> bindable property.
    /// </summary>
    public static readonly BindableProperty PlaceholderTextProperty =
        BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(CustomDropdown));

    /// <summary>
    ///     Identifies the <see cref="DisplayMemberPath" /> bindable property.
    /// </summary>
    public static readonly BindableProperty DisplayMemberPathProperty =
        BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(CustomDropdown));

    /// <summary>
    ///     Identifies the <see cref="CanEdit" /> bindable property.
    /// </summary>
    public static readonly BindableProperty CanEditProperty =
        BindableProperty.Create(nameof(CanEdit), typeof(bool), typeof(CustomDropdown), true);

    /// <summary>
    ///     Identifies the <see cref="IsFocused" /> bindable property.
    /// </summary>
    public static readonly BindableProperty ShowDropdownProperty =
        BindableProperty.Create(nameof(ShowDropdown), typeof(bool), typeof(CustomDropdown), false,
            BindingMode.TwoWay);

    /// <summary>
    ///     Identifies the <see cref="SelectedItem" /> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(CustomDropdown), null,
            BindingMode.TwoWay);

    /// <summary>
    ///     Identifies the <see cref="SelectedIndex" /> bindable property.
    /// </summary>
    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(CustomDropdown), -1,
            BindingMode.TwoWay);

    /// <summary>
    ///     Identifies the <see cref="InputViewStyle" /> bindable property.
    /// </summary>
    public static readonly BindableProperty InputViewStyleProperty =
        BindableProperty.Create(nameof(InputViewStyle), typeof(Style), typeof(CustomDropdown),
            default(Style), BindingMode.TwoWay);

    /// <summary>
    ///     Gets or sets the dropdown itemsource of the property that is displayed
    /// </summary>
    /// <value>
    ///     The dropdown itemsource of the property that is displayed in
    ///     the control. The default is null
    /// </value>
    public IList ItemSource
    {
        get => (IList)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    /// <summary>
    ///     Gets or sets the placeholder of the property that is displayed for the control
    /// </summary>
    /// <value>
    ///     The placeholder of the property that is displayed in
    ///     the control. The default is an empty string ("")
    /// </value>
    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    /// <summary>
    ///     Gets or sets the name or path of the property that is displayed for each data item
    /// </summary>
    /// <value>
    ///     the name or path of the property that is displayed for each data item in
    ///     the control. The default is an empty string ("")
    /// </value>
    public string DisplayMemberPath
    {
        get => (string)GetValue(DisplayMemberPathProperty);
        set => SetValue(DisplayMemberPathProperty, value);
    }

    /// <summary>
    ///     Gets or sets the edit property of the control
    /// </summary>
    /// <value>
    ///     The default is true
    /// </value>
    public bool CanEdit
    {
        get => (bool)GetValue(CanEditProperty);
        set => SetValue(CanEditProperty, value);
    }

    /// <summary>
    ///     Gets or sets the selected item for the control
    /// </summary>
    /// <value>
    ///     The default is null
    /// </value>
    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    ///     Gets or sets the selected index for the control
    /// </summary>
    /// <value>
    ///     The default is -1
    /// </value>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    ///     Gets or sets the input view style for the control
    /// </summary>
    public Style InputViewStyle
    {
        get => (Style)GetValue(InputViewStyleProperty);
        set => SetValue(InputViewStyleProperty, value);
    }

    public bool ShowDropdown
    {
        get => (bool)GetValue(ShowDropdownProperty);
        set => SetValue(ShowDropdownProperty, value);
    }

    #endregion

    #region Events

    public event EventHandler<string> TextChanged;
    public event EventHandler<ItemSelectedEventArgs> ItemSelected;

    #endregion
}

public class SelectedIndexChangedEventArgs : EventArgs
{
    public SelectedIndexChangedEventArgs(int oldIndex, int newIndex)
    {
        OldIndex = oldIndex;
        NewIndex = newIndex;
    }

    public int OldIndex { get; }
    public int NewIndex { get; set; }
}

public class ItemSelectedEventArgs : EventArgs
{
    public int SelectedIndex { get; set; }
}