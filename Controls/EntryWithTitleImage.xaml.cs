using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using AirIQ.Helpers;
using AirIQ.Resources.Strings;
using Mopups.Interfaces;

namespace AirIQ.Controls;

public partial class EntryWithTitleImage : ContentView
{
	public EntryWithTitleImage()
	{
		InitializeComponent();

		dropdown.SetBinding(CustomDropdown.ShowDropdownProperty,
					new Binding(nameof(OnLoadShowDropdown), BindingMode.TwoWay, source: this));

		Loaded += (_, _) =>
		{
			UpdateFieldVisibility();
			RefreshNavigationLabel();
		};
	}

	/// <summary>
	///     Identifies the <see cref="Title" /> bindable property.
	/// </summary>
	public static readonly BindableProperty TitleProperty =
		BindableProperty.Create(nameof(Title), typeof(string), typeof(EntryWithTitleImage));

	/// <summary>
	///     Identifies the <see cref="PlaceholderText" /> bindable property.
	/// </summary>
	public static readonly BindableProperty PlaceholderTextProperty =
		BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(EntryWithTitleImage));

	/// <summary>
	///     Identifies the <see cref="ItemsSource" /> bindable property.
	/// </summary>
	public static readonly BindableProperty ItemsSourceProperty =
		BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(EntryWithTitleImage),
			default(IList), BindingMode.TwoWay);

	/// <summary>
	///     Identifies the <see cref="SelectedItem" /> bindable property.
	/// </summary>
	public static readonly BindableProperty SelectedItemProperty =
		BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(EntryWithTitleImage), null,
			BindingMode.TwoWay, propertyChanged: OnSelectedItemChanged);

	/// <summary>
	///     Identifies the <see cref="ErrorText" /> bindable property.
	/// </summary>
	public static readonly BindableProperty ErrorTextProperty =
		BindableProperty.Create(nameof(ErrorText), typeof(string), typeof(EntryWithTitleImage),
			default(string), BindingMode.TwoWay);

	/// <summary>
	///     Identifies the <see cref="DisplayMemberPath" /> bindable property.
	/// </summary>
	public static readonly BindableProperty DisplayMemberPathProperty =
		BindableProperty.Create(nameof(DisplayMemberPath), typeof(string), typeof(EntryWithTitleImage));

	/// <summary>
	///     Identifies the <see cref="CanEdit" /> bindable property.
	/// </summary>
	public static readonly BindableProperty CanEditProperty =
		BindableProperty.Create(nameof(CanEdit), typeof(bool), typeof(EntryWithTitleImage), false);

	/// <summary>
	///     Identifies the <see cref="OnLoadShowDropdown" /> bindable property.
	/// </summary>
	public static readonly BindableProperty OnLoadShowDropdownProperty =
		BindableProperty.Create(nameof(OnLoadShowDropdown), typeof(bool), typeof(EntryWithTitleImage),
			false, BindingMode.TwoWay);

	/// <summary>
	///     set icon for the control
	/// </summary>
	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(EntryWithTitleImage),
			null, BindingMode.TwoWay);

	/// <summary>
	///    Identifies the <see cref="IsError" /> bindable property.
	/// </summary>
	public static readonly BindableProperty IsErrorProperty =
		BindableProperty.Create(nameof(IsError), typeof(bool), typeof(EntryWithTitleImage), false,
			BindingMode.TwoWay);

	/// <summary>
	///     set sub-title for the control
	/// </summary>
	public static readonly BindableProperty IsFontImagePoperty =
		BindableProperty.Create(nameof(IsFontImage), typeof(bool), typeof(EntryWithTitleImage), true,
			BindingMode.TwoWay);

	public static readonly BindableProperty InputViewStyleProperty =
		BindableProperty.Create(nameof(InputViewStyle), typeof(Style), typeof(EntryWithTitleImage));

	public static readonly BindableProperty TextChangeCommandProperty =
	BindableProperty.Create(nameof(TextChangeCommand), typeof(ICommand), typeof(EntryWithTitleImage));

	public static readonly BindableProperty IsDateViewVisibleProperty =
			BindableProperty.Create(nameof(IsDateViewVisible), typeof(bool), typeof(EntryWithTitleImage), false,
				BindingMode.TwoWay, propertyChanged: (bindable, _, _) => ((EntryWithTitleImage)bindable).UpdateFieldVisibility());

	/// <summary>
	///     Identifies the <see cref="IsNavigationField" /> bindable property.
	/// </summary>
	public static readonly BindableProperty IsNavigationFieldProperty =
		BindableProperty.Create(nameof(IsNavigationField), typeof(bool), typeof(EntryWithTitleImage), false,
			propertyChanged: (bindable, _, _) =>
			{
				var control = (EntryWithTitleImage)bindable;
				control.UpdateFieldVisibility();
				control.RefreshNavigationLabel();
			});

	/// <summary>
	///     Identifies the <see cref="NavigationCommand" /> bindable property.
	/// </summary>
	public static readonly BindableProperty NavigationCommandProperty =
		BindableProperty.Create(nameof(NavigationCommand), typeof(ICommand), typeof(EntryWithTitleImage));

	/// <summary>
	///     Identifies the <see cref="NavigationCommandParameter" /> bindable property.
	/// </summary>
	public static readonly BindableProperty NavigationCommandParameterProperty =
		BindableProperty.Create(nameof(NavigationCommandParameter), typeof(object), typeof(EntryWithTitleImage));
	public static readonly BindableProperty DateProperty =
	   BindableProperty.Create(propertyName: nameof(Date), returnType: typeof(DateTime?), declaringType: typeof(EntryWithTitleImage), null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>
	   {
		   var control = (EntryWithTitleImage)bindable;
		   if (newValue != null && (DateTime)newValue != default)
		   {
			   control.datelbl.Text = ((DateTime)newValue).ToString("dd/MM/yyyy");
			   control.datelbl.TextColor = Colors.Black;
		   }
		   else if (newValue == null)
		   {
			   control.datelbl.Text = AppResource.DateFormatPlaceholder;
		   }
	   });

	public static readonly BindableProperty AllowedDatesProperty =
		BindableProperty.Create(nameof(AllowedDates), typeof(IList<DateTime>), typeof(EntryWithTitleImage), null, BindingMode.TwoWay);

	public IList<DateTime> AllowedDates
	{
		get => (IList<DateTime>)GetValue(AllowedDatesProperty);
		set => SetValue(AllowedDatesProperty, value);
	}

	public DateTime? Date
	{
		get { return (DateTime?)GetValue(DateProperty); }
		set
		{
			SetValue(DateProperty, value);

		}
	}

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

	public bool IsDateViewVisible
	{
		get => (bool)GetValue(IsDateViewVisibleProperty);
		set => SetValue(IsDateViewVisibleProperty, value);
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

	public bool IsNavigationField
	{
		get => (bool)GetValue(IsNavigationFieldProperty);
		set => SetValue(IsNavigationFieldProperty, value);
	}

	public ICommand NavigationCommand
	{
		get => (ICommand)GetValue(NavigationCommandProperty);
		set => SetValue(NavigationCommandProperty, value);
	}

	public object NavigationCommandParameter
	{
		get => GetValue(NavigationCommandParameterProperty);
		set => SetValue(NavigationCommandParameterProperty, value);
	}

	#region Events



	public event EventHandler SelectedIndexChanged;

	#endregion

	private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = bindable as EntryWithTitleImage;
		control?.RefreshNavigationLabel();
	}

	void LifecycleEffectLoaded(object sender, EventArgs e)
	{
		dropdown.ShowDropdown = OnLoadShowDropdown;
	}

	void UpdateFieldVisibility()
	{
		dropdown.IsVisible = !IsDateViewVisible && !IsNavigationField;
		datelbl.IsVisible = IsDateViewVisible;
		navigationLabel.IsVisible = IsNavigationField;
	}

	void RefreshNavigationLabel()
	{
		if (!IsNavigationField)
			return;

		string selectedText = string.Empty;
		if (SelectedItem != null)
		{
			selectedText = string.IsNullOrEmpty(DisplayMemberPath)
				? SelectedItem.ToString()
				: SelectedItem.GetType().GetProperty(DisplayMemberPath)?.GetValue(SelectedItem)?.ToString() ?? string.Empty;
		}

		if (string.IsNullOrEmpty(selectedText))
		{
			navigationLabel.Text = PlaceholderText;
			navigationLabel.TextColor = (Color)Application.Current!.Resources["Gray50"];
		}
		else
		{
			navigationLabel.Text = selectedText;
			navigationLabel.TextColor = (Color)Application.Current!.Resources["Gray90"];
		}
	}

	void TapGestureRecognizerTapped(object sender, EventArgs e)
	{
		if (IsDateViewVisible)
		{
			var popupNavigation = ServiceHelper.GetService<IPopupNavigation>();
			if (popupNavigation != null)
			{
				var popup = new CalendarViewV2();
				popup.SetBinding(CalendarViewV2.AllowedDatesProperty,
						new Binding(nameof(AllowedDates), BindingMode.TwoWay, source: this));
				popup.DatePicked += (arg) =>
				{
					Date = arg;
				};
				popupNavigation?.PushAsync(popup);
			}
		}
		else if (IsNavigationField)
		{
			if (NavigationCommand?.CanExecute(NavigationCommandParameter) == true)
			{
				NavigationCommand.Execute(NavigationCommandParameter);
			}
		}
		else
		{
			dropdown.ShowDropdown = !dropdown.ShowDropdown;
		}
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