using AirIQ.Constants;
using AirIQ.Controls;
using AirIQ.Platforms.iOS.Handlers;
using Microsoft.Maui.Handlers;
using UIKit;

namespace AirIQ.Platforms.Handlers
{
#nullable disable
    public class CustomDropdownHandler : ViewHandler<CustomDropdown, IosDropDownBox>
    {
        private static readonly int baseHeight = 10;
        private string fontFamily;
        private CustomDropdown customDropdown;

        public IosDropDownBox Box { get; private set; }

        public static readonly IPropertyMapper<CustomDropdown, CustomDropdownHandler> PropertyMapper = new PropertyMapper<CustomDropdown, CustomDropdownHandler>(ViewHandler.ViewMapper)
        {

        };

        public CustomDropdownHandler() : base(PropertyMapper)
        {
        }

        public override Size GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            SizeRequest baseResult = base.GetDesiredSize(widthConstraint, heightConstraint);
            Foundation.NSString testString = new Foundation.NSString("Tj");
            CoreGraphics.CGSize testSize =
                testString.GetSizeUsingAttributes(new UIStringAttributes { Font = Box.Font });
            double height = baseHeight + testSize.Height;
            height = Math.Round(height);
            return new Size(baseResult.Request.Width, height);
        }

        protected override IosDropDownBox CreatePlatformView()
        {
            customDropdown = VirtualView;
            Box = new IosDropDownBox(customDropdown.CanEdit)
            {
                customDropdown = customDropdown
            };
            DrawNativeControl();
            UpdatePlaceholderText();
            UpdateIsEnabled();
            UpdateSelectedIndex();

            return Box;
        }

        protected override void ConnectHandler(IosDropDownBox platformView)
        {
            base.ConnectHandler(platformView);
            if (this.VirtualView != null)
            {
                this.VirtualView.PropertyChanged += VirtualView_PropertyChanged;
                platformView.SuggestionChosen += OnItemSelected;
                platformView.DropdownVisibiltyChanged += AutoSuggestBox_DropdownVisibiltyChanged;
                platformView.TextChanged += AutoSuggestBox_TextChanged;
            }
        }

        protected override void DisconnectHandler(IosDropDownBox platformView)
        {
            base.DisconnectHandler(platformView);
            if (this.VirtualView != null)
            {
                this.VirtualView.PropertyChanged -= VirtualView_PropertyChanged;
                platformView.SuggestionChosen -= OnItemSelected;
                platformView.DropdownVisibiltyChanged -= AutoSuggestBox_DropdownVisibiltyChanged;
                platformView.TextChanged -= AutoSuggestBox_TextChanged;
            }
        }

        #region [Methods]
        private void UpdateSelectedIndex()
        {
            if (customDropdown.SelectedItem != null)
            {
                Box.Text = FormatType(customDropdown.SelectedItem, customDropdown.DisplayMemberPath);
            }
        }

        private void UpdateIsEnabled()
        {
            Box.UserInteractionEnabled = customDropdown.IsEnabled;
            if (!customDropdown.IsEnabled)
            {
                Box.SetPlaceholderTextColor((Color)Application.Current.Resources["RoyalMailDarkGrey"]);
            }
            else
            {
                Box.SetPlaceholderTextColor((Color)Application.Current.Resources["RoyalMailBlack"]);
            }
        }

        private void UpdateDisplayMemberPath()
        {
            Box.SetItems(customDropdown.ItemSource?.OfType<object>(),
                (o) => FormatType(o, customDropdown?.DisplayMemberPath));
        }

        private void UpdateItemsSource()
        {
            Box.SetItems(customDropdown.ItemSource?.OfType<object>(),
                (o) => FormatType(o, customDropdown?.DisplayMemberPath));
            Box.IsSuggestionListOpen = customDropdown.ShowDropdown;
        }

        private static string FormatType(object instance, string memberPath)
        {
            if (!string.IsNullOrEmpty(memberPath) && instance?.GetType() != typeof(string))
                return instance?.GetType().GetProperty(memberPath)?.GetValue(instance)?.ToString() ?? "";
            else
                return instance?.ToString() ?? "";
        }

        private void UpdatePlaceholderText() => Box.PlaceholderText = customDropdown.PlaceholderText;

        private void DrawNativeControl()
        {
            var viewController = UIApplication.SharedApplication.KeyWindow?.RootViewController;
            while (viewController != null && viewController.PresentedViewController != null)
            {
                viewController = viewController.PresentedViewController;
            }

            var ctrl = viewController;

            var relativePosition = UIApplication.SharedApplication.KeyWindow;

            Box.Superview?.ConvertRectToView(Box.Frame, relativePosition);
            Box.Draw(ctrl, Box.Layer, null);
            UpdateItemsSource();
        }

        #endregion

        #region [Events]

        private void VirtualView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Box == null)
            {
                return;
            }
            else if (e.PropertyName == nameof(CustomDropdown.PlaceholderText))
            {
                UpdatePlaceholderText();
            }
            else if (e.PropertyName == nameof(CustomDropdown.DisplayMemberPath))
            {
                UpdateDisplayMemberPath();
            }
            else if (e.PropertyName == nameof(CustomDropdown.IsEnabled))
            {
                UpdateIsEnabled();
            }
            else if (e.PropertyName == nameof(CustomDropdown.ItemSource))
            {
                var items = ((CustomDropdown)sender).ItemSource;
                if (items != null && items.Count == 0)
                {
                    items.Add(StringConstants.NoResultAvailable);
                }
                UpdateItemsSource();
            }
            else if (e.PropertyName == CustomDropdown.SelectedItemProperty.PropertyName)
            {
                UpdateSelectedIndex();
            }
        }

        private void OnItemSelected(object? sender, AutoSuggestBoxSuggestionChosenEventArgs e)
        {
            if (customDropdown.SelectedItem != e.SelectedItem)
            {
                customDropdown.SelectedItem = e.SelectedItem;
            }
            customDropdown.ShowDropdown = false;
        }

        private void AutoSuggestBox_DropdownVisibiltyChanged(object? sender, bool value)
        {
            customDropdown.ShowDropdown = value;
        }

        private void AutoSuggestBox_TextChanged(object? sender, string text)
        {
            customDropdown.OnTextChanged(text);
        }
        #endregion
    }
}
