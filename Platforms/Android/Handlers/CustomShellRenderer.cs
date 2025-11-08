using Android.Graphics.Drawables;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Controls.Platform.Compatibility;
using Microsoft.Maui.Platform;
using Animation = Android.Views.Animations;
using View = Android.Views.View;


namespace AirIQ.Platforms.Handlers
{
    public class CustomShellRenderer : ShellRenderer
    {

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem item)
        {
            return new CustomBottomNavAppearance(this, item);
        }

    }

    class CustomBottomNavAppearance : ShellBottomNavViewAppearanceTracker
    {
        View? tabIndicatorView;
        private int indicatorWidth;

        FrameLayout.LayoutParams? indicatorViewParameter { get; set; }

        private BottomNavigationView? bottomNavigationView;
        public bool IsIndicatorAdded { get; private set; }
        private readonly ShellItem shellItem;

        public CustomBottomNavAppearance(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
            this.shellItem = shellItem;
        }

        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            bottomNavigationView = bottomView;
            bottomView.Elevation = 20; // adds shadow
            bottomView.SetBackgroundColor(Colors.White.ToPlatform());

            if (!IsIndicatorAdded)
            {
                AddIndicatorView();
                IsIndicatorAdded = true;
            }

            // Observe when Shell changes the active tab
            shellItem.PropertyChanged += OnShellItemChanged;
        }

        private void OnShellItemChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ShellItem.CurrentItem))
            {
                int index = shellItem.Items.IndexOf(shellItem.CurrentItem);
                if (index >= 0)
                    AnimateIndicatorView(index);
            }
        }
        private void BottomView_ItemSelected(object? sender, Google.Android.Material.Navigation.NavigationBarView.ItemSelectedEventArgs e)
        {
            AnimateIndicatorView(e.Item.ItemId);
        }

        void AddIndicatorView()
        {
            tabIndicatorView = new View(Platform.CurrentActivity);

            int itemCount = bottomNavigationView!.Menu.Size();
            var indicatorHeight = 10;
            indicatorWidth = bottomNavigationView.ItemIconSize * itemCount;
            indicatorViewParameter = new FrameLayout.LayoutParams(indicatorWidth, indicatorHeight);
            tabIndicatorView.LayoutParameters = indicatorViewParameter;

            SetGradient();

            // ✅ Add to BottomNavigationView after ensuring it's attached
            bottomNavigationView.Post(() =>
            {
                bottomNavigationView.AddView(tabIndicatorView);
            });


        }

        public void AnimateIndicatorView(int position)
        {
            if (tabIndicatorView == null) return;


            int navWidth = bottomNavigationView.Width;
            int itemCount = bottomNavigationView.Menu.Size();
            int tabWidth = navWidth / itemCount;

            tabIndicatorView?.Animate()?
                .TranslationX(tabWidth*position)
                .SetDuration(300)
                .SetInterpolator(new Animation.DecelerateInterpolator())
                .Start();
        }

        void SetGradient()
        {
            var startColor = Color.FromArgb("#F26B6C");
            var endColor = Color.FromArgb("#FF8A80");
            int[] colors = { startColor.ToPlatform(), endColor.ToPlatform() };

            GradientDrawable gradientDrawable = new GradientDrawable(GradientDrawable.Orientation.TopBottom, colors);

            gradientDrawable.SetCornerRadius(20);
            gradientDrawable.SetGradientType(GradientType.LinearGradient);

            tabIndicatorView.SetBackground(gradientDrawable);
        }
    }

}
