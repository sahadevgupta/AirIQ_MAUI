using CoreAnimation;
using CoreGraphics;
using AirIQ.Constants;
using AirIQ.Controls;
using Foundation;
using Microsoft.Maui.Platform;
using UIKit;

namespace AirIQ.Platforms.iOS.Handlers
{
    /// <summary>
    /// Creates a UIView with dropdown with a similar API and behavior to UWP's AutoSuggestBox
    /// </summary>
    [Register("UIAutoCompleteTextField")]
    public class IosDropDownBox : UIView, IUITextFieldDelegate
    {
        private Func<object, string> textFunc;
        private static CGRect selectionListFrame { get; set; }
        private UIViewController _parentViewController;
        public CustomDropdown customDropdown;
        private bool suppressTextChangedEvent;
        private NSObject _keyboardShowObserver;
        private NSObject _keyboardHideObserver;
        private CGRect _keyboardFrame;
        public int AutocompleteTableViewHeight { get; set; } = 150;
        public static List<IosDropDownBox> Instances = [];
        public UITextField InputTextField { get; }
        private bool _isSuggestionListOpen;
        private UITableViewController _vc;


        public AutoCompleteTableView SelectionList { get; set; }
        public virtual UIFont Font
        {
            get => InputTextField.Font;
            set => InputTextField.Font = value ?? UIFont.PreferredBody;
        }
        public virtual string PlaceholderText
        {
            get => InputTextField.Placeholder;
            set => InputTextField.Placeholder = value;
        }
        public virtual bool IsSuggestionListOpen
        {
            get => _isSuggestionListOpen;
            set
            {
                _isSuggestionListOpen = value;
                DropdownVisibiltyChanged?.Invoke(this, value);
                UpdateSuggestionListOpenState();
            }
        }
        public virtual bool UpdateTextOnSelect { get; set; } = true;
        public virtual string Text
        {
            get => InputTextField.Text;
            set => InputTextField.Text = value;
        }

        public event EventHandler<bool> DropdownVisibiltyChanged;
        public event EventHandler<string>? TextChanged;
        public event EventHandler<AutoSuggestBoxSuggestionChosenEventArgs> SuggestionChosen;

        public IosDropDownBox(bool isEditable)
        {
            InputTextField = new UITextField
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BorderStyle = UITextBorderStyle.None,
                ReturnKeyType = UIReturnKeyType.Search,
                AutocorrectionType = UITextAutocorrectionType.No,
                BackgroundColor = Colors.Transparent.ToPlatform()
            };
            var leftPaddingView = new UIView(new CGRect(0, 0, 16, InputTextField.Frame.Height));
            var rightPaddingView = new UIView(new CGRect(0, 0, 38, InputTextField.Frame.Height));

            InputTextField.LeftView = leftPaddingView;
            InputTextField.LeftViewMode = UITextFieldViewMode.Always;
            InputTextField.RightView = rightPaddingView;
            InputTextField.RightViewMode = UITextFieldViewMode.Always;

            InputTextField.EditingDidBegin += InputTextField_EditingDidBegin;
            InputTextField.EditingDidEnd += InputTextField_EditingDidEnd;
            InputTextField.ShouldReturn = InputText_OnShouldReturn;

            if (isEditable)
            {
                InputTextField.EditingChanged += TextField_EditingChanged;
            }
            else
            {
                UITapGestureRecognizer tapGesture = new UITapGestureRecognizer(OnTapped);
                tapGesture.CancelsTouchesInView = true;
                InputTextField.AddGestureRecognizer(tapGesture);
            }

            AddSubview(InputTextField);
            InputTextField.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            InputTextField.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            InputTextField.WidthAnchor.ConstraintEqualTo(WidthAnchor).Active = true;
            InputTextField.HeightAnchor.ConstraintEqualTo(HeightAnchor).Active = true;

            Instances.Add(this);
            SubscribeToKeyboardNotifications();
        }

        public void Draw(UIViewController viewController, CALayer layer, UIScrollView scrollView)
        {
            _parentViewController = viewController;
            var tapGesture = new UITapGestureRecognizer(OnOutsideTap)
            {
                CancelsTouchesInView = false
            };

            _parentViewController?.View?.AddGestureRecognizer(tapGesture);

            if (SelectionList == null)
            {
                SelectionList = new AutoCompleteTableView()
                {
                    Hidden = true,
                };

                SelectionList.Layer.CornerRadius = 4;
                SelectionList.Layer.BorderColor = ((Color)Application.Current.Resources["LightGray"]).ToCGColor();
                SelectionList.Layer.BorderWidth = 1;

                _parentViewController?.View?.AddSubview(SelectionList);
            }
        }

        public void CloseExistingOpenDropdown()
        {
            if (Instances.Count > 1 && Instances.Any(x => x.IsSuggestionListOpen))
            {
                var data = Instances.FirstOrDefault(x => x.IsSuggestionListOpen);
                data!.IsSuggestionListOpen = false;
            }
        }

        public virtual void SetPlaceholderTextColor(Color color)
        {
            InputTextField.AttributedPlaceholder = new NSAttributedString(
                InputTextField.Placeholder ?? string.Empty,
                new UIStringAttributes
                {
                    ForegroundColor = ((Color)Application.Current.Resources["DarkGray"]).ToPlatform()
                });
        }

        public virtual void SetTextColor(Color color)
        {
            InputTextField.TextColor = color.ToPlatform();
        }

        public override bool BecomeFirstResponder()
        {
            return InputTextField.BecomeFirstResponder();
        }

        public override bool ResignFirstResponder()
        {
            return InputTextField.ResignFirstResponder();
        }

        public override bool IsFirstResponder => InputTextField.IsFirstResponder;

        internal void SetItems(IEnumerable<object> items, Func<object, string> labelFunc)
        {
            this.textFunc = labelFunc;
            if (SelectionList is null)
                return;
            if (SelectionList.Source is TableSource<object> oldSource)
            {
                oldSource.TableRowSelected -= SuggestionTableSource_TableRowSelected;
            }

            SelectionList.Source = null;
            IEnumerable<object> suggestions = items?.OfType<object>();
            if (suggestions != null && suggestions.Any())
            {
                TableSource<object> suggestionTableSource =
                    new(suggestions, labelFunc);
                suggestionTableSource.TableRowSelected += SuggestionTableSource_TableRowSelected;
                SelectionList.Source = suggestionTableSource;

                SelectionList.ReloadData();
                SelectionList.Hidden = !_isSuggestionListOpen;
                UpdateDropdownFrame();
            }
            else
            {
                IsSuggestionListOpen = false;
                SelectionList.Hidden = true;
            }
        }

        protected void SubscribeToKeyboardNotifications()
        {
            _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow((sender, args) =>
            {
                _keyboardFrame = args.FrameEnd;
                AdjustDropdownForKeyboard();
            });

            _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide((sender, args) =>
            {
                _keyboardFrame = CGRect.Empty;
                AdjustDropdownForKeyboard();
            });
        }

        private void AdjustDropdownForKeyboard()
        {
            if (SelectionList == null || SelectionList.Hidden) return;

            var view = _parentViewController?.View;
            if (view == null) return;

            nfloat maxY = _keyboardFrame.IsEmpty ? view.Frame.Bottom : _keyboardFrame.Top;
            var dropdownFrame = SelectionList.Frame;

            if (dropdownFrame.Bottom > maxY)
            {
                var newY = maxY - dropdownFrame.Height - 50;
                if (newY < 0) newY = 0;
                dropdownFrame.Y = newY;
                SelectionList.Frame = dropdownFrame;
            }
        }

        private bool InputText_OnShouldReturn(UITextField textField)
        {
            if (string.IsNullOrWhiteSpace(textField.Text)) { return false; }
            textField.ResignFirstResponder();
            return true;
        }

        private void InputTextField_EditingDidEnd(object? sender, EventArgs e)
        {
            IsSuggestionListOpen = false;
        }

        private void InputTextField_EditingDidBegin(object? sender, EventArgs e)
        {
            IsSuggestionListOpen = true;
        }

        private void TextField_EditingChanged(object? sender, EventArgs e)
        {
            if (!suppressTextChangedEvent)
                this.TextChanged?.Invoke(this, Text);

            IsSuggestionListOpen = true;
        }

        private void OnOutsideTap(UITapGestureRecognizer gesture)
        {
            var location = gesture.LocationInView(_parentViewController.View);

            if (!Frame.Contains(location) && (SelectionList == null || !selectionListFrame.Contains(location)))
            {
                CloseExistingOpenDropdown();
            }
        }

        private void OnTapped()
        {
            CloseExistingOpenDropdown();
            IsSuggestionListOpen = true;
            DropdownVisibiltyChanged?.Invoke(this, true);
        }

        private void UpdateSuggestionListOpenState()
        {
            if (SelectionList == null) return;

            SelectionList.Hidden = !_isSuggestionListOpen;
            UpdateDropdownFrame();
        }

        private void OnEditingDidBegin(object sender, EventArgs e)
        {
            CloseExistingOpenDropdown();
            IsSuggestionListOpen = true;
        }

        private void OnEditingDidEnd(object sender, EventArgs e)
        {
            IsSuggestionListOpen = false;
        }

        private void SuggestionTableSource_TableRowSelected(object? sender, TableRowSelectedEventArgs<object> e)
        {
            suppressTextChangedEvent = true;
            SelectionList.DeselectRow(e.SelectedItemIndexPath, false);
            object selection = e.SelectedItem;
            string selectedItem = textFunc(selection);
            if (UpdateTextOnSelect)
            {
                if (selectedItem == StringConstants.NoResultAvailable)
                {
                    InputTextField.Text = string.Empty;
                }
                else
                {
                    InputTextField.Text = textFunc(selection);
                    InputTextField.TextColor = ((Color)Application.Current.Resources["Black"]).ToPlatform();
                }
            }
            suppressTextChangedEvent = false;
            SuggestionChosen?.Invoke(this, new AutoSuggestBoxSuggestionChosenEventArgs(selectedItem == StringConstants.NoResultAvailable ? null : selection));
            IsSuggestionListOpen = false;
            InputTextField.ResignFirstResponder();
        }

        private void InputText_EditingChanged(object sender, EventArgs e)
        {
            IsSuggestionListOpen = true;
        }

        private void UpdateDropdownFrame()
        {
            if (SelectionList == null || SelectionList.Hidden) return;

            var relativePosition = UIApplication.SharedApplication?.KeyWindow;
            var view = _parentViewController?.View;
            if (view == null) return;

            var result = new Point(customDropdown.X, customDropdown.Y);
            VisualElement visualview = customDropdown;
            while (visualview.Parent is VisualElement parent)
            {
                result = result.Offset(parent.X, parent.Y);
                visualview = parent;
            }
            CGRect relativeFrame = new CGRect();
            if (this.Superview != null)
            {
                relativeFrame = this.Superview.ConvertRectToView(this.Frame, relativePosition);
            }

            var height = Math.Min(AutocompleteTableViewHeight, (int)SelectionList.ContentSize.Height);
            SelectionList.Frame = new CGRect(
                relativeFrame.X,
                relativeFrame.Y + this.Frame.Height + 20,
                InputTextField.Frame.Width, height);

            selectionListFrame = SelectionList.Frame;
            AdjustDropdownForKeyboard();
        }

        private class TableSource<T> : UITableViewSource
        {
            private readonly IEnumerable<T> _items;
            private readonly Func<T, string> _labelFunc;
            private readonly string _cellIdentifier;

            public TableSource(IEnumerable<T> items, Func<T, string> labelFunc)
            {
                _items = items;
                _labelFunc = labelFunc;
                _cellIdentifier = Guid.NewGuid().ToString();
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                UITableViewCell cell = tableView.DequeueReusableCell(_cellIdentifier);
                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, _cellIdentifier)
                    {
                        PreservesSuperviewLayoutMargins = true,
                        SeparatorInset = UIEdgeInsets.Zero,
                        LayoutMargins = UIEdgeInsets.Zero,
                        BackgroundColor = UIColor.White
                    };
                }

                T item = _items.ElementAt(indexPath.Row);
                string labelText = _labelFunc(item);

                cell.TextLabel.Text = labelText;
                cell.TextLabel.TextColor = labelText == StringConstants.NoResultAvailable
                    ? ((Color)Application.Current.Resources["Red"]).ToPlatform()
                    : ((Color)Application.Current.Resources["Black"]).ToPlatform();
                cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;
                cell.TextLabel.Lines = 0;

                return cell;
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                OnTableRowSelected(indexPath);
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return _items.Count();
            }

            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return UITableView.AutomaticDimension;
            }

            public event EventHandler<TableRowSelectedEventArgs<T>> TableRowSelected;

            private void OnTableRowSelected(NSIndexPath itemIndexPath)
            {
                T item = _items.ElementAt(itemIndexPath.Row);
                string label = _labelFunc(item);
                TableRowSelected?.Invoke(this,
                    new TableRowSelectedEventArgs<T>(item, label, itemIndexPath));
            }
        }

        private class TableRowSelectedEventArgs<T> : EventArgs
        {
            public TableRowSelectedEventArgs(
                T selectedItem,
                string selectedItemLabel,
                NSIndexPath selectedItemIndexPath
            )
            {
                SelectedItem = selectedItem;
                SelectedItemLabel = selectedItemLabel;
                SelectedItemIndexPath = selectedItemIndexPath;
            }

            public T SelectedItem { get; }
            public string SelectedItemLabel { get; }
            public NSIndexPath SelectedItemIndexPath { get; }
        }
    }

    public sealed class AutoSuggestBoxSuggestionChosenEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSuggestBoxSuggestionChosenEventArgs"/> class.
        /// </summary>
        /// <param name="selectedItem"></param>
        internal AutoSuggestBoxSuggestionChosenEventArgs(object selectedItem)
        {
            SelectedItem = selectedItem;
        }

        /// <summary>
        /// Gets a reference to the selected item.
        /// </summary>
        /// <value>A reference to the selected item.</value>
        public object SelectedItem { get; }
    }

    public class AutoCompleteTableView : UITableView
    {
        private readonly UIScrollView _parentScrollView;

        public AutoCompleteTableView()
        {
            
        }


        public override bool Hidden
        {
            get { return base.Hidden; }
            set
            {
                base.Hidden = value;
                if (_parentScrollView == null) return;
                _parentScrollView.DelaysContentTouches = !value;
            }
        }
    }
}
