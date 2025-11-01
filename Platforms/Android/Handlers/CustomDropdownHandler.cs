using AirIQ.Constants;
using AirIQ.Controls;
using AirIQ.Platforms.Android.Handlers;
using Android.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Core.Content;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using View = Android.Views.View;

namespace AirIQ.Platforms.Handlers
{
    public class CustomDropdownHandler : ViewHandler<CustomDropdown, AutoCompleteTextView>
    {
        private AutoCompleteTextView autoCompleteTextView;
        private CustomDropdown customDropdown;
        private SuggestCompleteAdapter adapter;
        private const string NoResultAvailable = "No results available";

        public static readonly IPropertyMapper<CustomDropdown, CustomDropdownHandler> PropertyMapper = new PropertyMapper<CustomDropdown, CustomDropdownHandler>(ViewHandler.ViewMapper)
        {

        };

        public CustomDropdownHandler() : base(PropertyMapper)
        {
        }

        protected override AutoCompleteTextView CreatePlatformView()
        {
            Context themedContext = new ContextThemeWrapper(Context,Resource.Style.MyTheme);

            autoCompleteTextView = new AutoCompleteTextView(themedContext);
            customDropdown = VirtualView;
            UpdateText();
            UpdateEnabledProperty();

            autoCompleteTextView.SetBackgroundColor(Colors.Transparent.ToPlatform());
            autoCompleteTextView.Hint = customDropdown.PlaceholderText;

            if (!customDropdown.CanEdit)
            {
                autoCompleteTextView.InputType = InputTypes.TextFlagNoSuggestions;
                autoCompleteTextView.SetFocusable(ViewFocusability.NotFocusable);
                autoCompleteTextView.Clickable = true;
            }

            autoCompleteTextView.Threshold = 0;
            autoCompleteTextView.SetHintTextColor(((Color)Application.Current.Resources["LightGray"]).ToPlatform());

            autoCompleteTextView.SetSingleLine(true);
            autoCompleteTextView.SetTextColor(((Color)Application.Current.Resources["DarkGray"]).ToPlatform());

            autoCompleteTextView.Click += AutoCompleteTextView_Click;
            autoCompleteTextView.ItemClick += AutoCompleteTextView_ItemClick;
            autoCompleteTextView.TextChanged += AutoCompleteTextView_TextChanged;
            autoCompleteTextView.FocusChange += AutoCompleteTextView_FocusChange;
            adapter = new SuggestCompleteAdapter(Context, Resource.Layout.autocomplete_list_row, Resource.Id.autocomplete_textview);

            UpdateItemsSource(customDropdown?.ItemSource?.OfType<object>());
            UpdateDropdownHeight(customDropdown?.ItemSource);

            var drawable = ContextCompat.GetDrawable(Context, Resource.Drawable.background_single_line);
            autoCompleteTextView.SetDropDownBackgroundDrawable(drawable);
            return autoCompleteTextView;
        }

        protected override void ConnectHandler(AutoCompleteTextView platformView)
        {
            base.ConnectHandler(platformView);
            if (this.VirtualView != null)
            {
                int leftPadding = (int)(12 * platformView.Context.Resources.DisplayMetrics.Density);
                int rightPadding = 100;
                platformView.SetPadding(leftPadding, platformView.PaddingTop, rightPadding, platformView.PaddingBottom);
                platformView.DropDownVerticalOffset = leftPadding;

                this.VirtualView.PropertyChanged += VirtualView_PropertyChanged;

                platformView.SetOnDismissListener(new DropdownDismissListener(() =>
                {
                    VirtualView.ShowDropdown = false;
                }));
            }
        }

        protected override void DisconnectHandler(AutoCompleteTextView platformView)
        {
            base.DisconnectHandler(platformView);
            if (this.VirtualView != null)
            {
                this.VirtualView.PropertyChanged -= VirtualView_PropertyChanged;
            }

            autoCompleteTextView.Click -= AutoCompleteTextView_Click;
            autoCompleteTextView.TextChanged -= AutoCompleteTextView_TextChanged;
            autoCompleteTextView.ItemClick -= AutoCompleteTextView_ItemClick;
            autoCompleteTextView.FocusChange -= AutoCompleteTextView_FocusChange;

            autoCompleteTextView.Dispose();
            customDropdown = null;
            adapter.Dispose();
        }

        private void VirtualView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == CustomDropdown.ItemSourceProperty.PropertyName)
            {
                var items = ((CustomDropdown)sender).ItemSource;
                if (items != null && items.Count == 0)
                {
                    items.Add(StringConstants.NoResultAvailable);
                }

                UpdateDropdownHeight(items);

                UpdateItemsSource(items?.OfType<object>());
            }
            else if (e.PropertyName == CustomDropdown.SelectedItemProperty.PropertyName)
            {
                UpdateText();
            }
            else if (e.PropertyName == CustomDropdown.ShowDropdownProperty.PropertyName)
            {
                UpdateDropdownVisibility();
            }
        }

        #region [ Methods ]

        private void UpdateDropdownHeight(System.Collections.IList? items)
        {
            var rowHeight = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 40, Platform.AppContext.Resources.DisplayMetrics);
            int maxHeight = rowHeight * 3;
            int dropdownHeight = items is null ? 350 : Math.Min(items!.Count * rowHeight, maxHeight);
            autoCompleteTextView.DropDownHeight = dropdownHeight;
            VirtualView.HeightRequest = dropdownHeight;
        }

        private void UpdateDropdownVisibility()
        {
            if (customDropdown == null) return;
            if (customDropdown.ItemSource != null && customDropdown.ItemSource.Count > 0)
            {
                if (customDropdown.ShowDropdown)
                {
                    autoCompleteTextView.ShowDropDown();

                }
                else
                {
                    autoCompleteTextView.DismissDropDown();
                }
            }
        }

        void UpdateShowDropdown()
        {
            customDropdown.ShowDropdown = autoCompleteTextView.IsPopupShowing;
        }

        private async void UpdateItemsSource(IEnumerable<object>? items)
        {
            adapter.UpdateList(items == null ? Enumerable.Empty<string>() : items.OfType<object>(),
                                   (o) => FormatType(o, customDropdown.DisplayMemberPath));
            UpdateAdapter();
            await Task.Delay(1000);
        }

        private void UpdateAdapter()
        {
            try
            {
                autoCompleteTextView.Adapter = adapter;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        private void UpdateEnabledProperty()
        {
            autoCompleteTextView.Enabled = customDropdown.IsEnabled;
        }

        private void UpdateText()
        {
            if (customDropdown.SelectedItem is object)
            {
                string value = FormatType(customDropdown.SelectedItem, customDropdown.DisplayMemberPath);
                autoCompleteTextView.SetText(value, true);
                autoCompleteTextView.SetTextColor(((Color)Application.Current.Resources["DarkGray"]).ToPlatform());
            }
        }

        private string FormatType(object instance, string displayMemberPath)
        {
            if (!string.IsNullOrEmpty(displayMemberPath) && instance.GetType() != typeof(string))
            {
                return instance?.GetType().GetProperty(displayMemberPath)?.GetValue(instance)?.ToString() ?? "";
            }
            else
            {
                return instance?.ToString() ?? "";
            }
        }

        #endregion

        #region [ Events ]

        private void AutoCompleteTextView_FocusChange(object? sender, global::Android.Views.View.FocusChangeEventArgs e)
        {
            autoCompleteTextView.ShowDropDown();
            UpdateShowDropdown();
        }

        private void AutoCompleteTextView_ItemClick(object? sender, AdapterView.ItemClickEventArgs e)
        {
            var selectedItem = customDropdown.ItemSource?.OfType<object>().ElementAt(e.Position);
            if (selectedItem == StringConstants.NoResultAvailable)
            {
                autoCompleteTextView.Text = string.Empty;
            }
            else if (customDropdown.SelectedItem != selectedItem)
            {
                customDropdown.OnItemSelected(selectedItem);
                autoCompleteTextView.ClearFocus();
                HideKeyboard(autoCompleteTextView);
            }
        }

        private void HideKeyboard(View view)
        {
            var imm = (InputMethodManager)MauiApplication.Current.GetSystemService(Context.InputMethodService);

            if (imm != null)
            {
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }

        private void AutoCompleteTextView_TextChanged(object? sender, global::Android.Text.TextChangedEventArgs e)
        {
            if (!autoCompleteTextView.IsPerformingCompletion)
            {
                customDropdown.OnTextChanged(e.Text!);
            }
        }

        private void AutoCompleteTextView_Click(object? sender, EventArgs e)
        {
            autoCompleteTextView.ShowDropDown();
            UpdateShowDropdown();
        }

        #endregion

    }

    class DropdownDismissListener : Java.Lang.Object, AutoCompleteTextView.IOnDismissListener
    {
        readonly Action _onDismiss;
        public DropdownDismissListener(Action onDismiss)
        {
            _onDismiss = onDismiss;
        }

        public void OnDismiss()
        {
            _onDismiss?.Invoke();
        }
    }
}

