using Microsoft.Maui.Controls;
using UIKit;
using CoreGraphics;

namespace AirIQ.Platforms.Handlers
{
    public static class TabBarIOSCustomization
    {
        public static void CustomizeTabBar(Shell shell)
        {
#if IOS
            shell.Loaded += (s, e) =>
            {
                AddTabBarBorder(shell);
            };
#endif
        }

        private static void AddTabBarBorder(Shell shell)
        {
#if IOS
            try
            {
                var handler = shell.Handler as IElementHandler;
                if (handler?.PlatformView is UIView platformView)
                {
                    FindAndStyleTabBar(platformView);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error customizing iOS TabBar: {ex.Message}");
            }
#endif
        }

        private static void FindAndStyleTabBar(UIView view)
        {
#if IOS
            if (view is UITabBar tabBar)
            {
                // Set white background
                tabBar.BarTintColor = UIColor.White;
                tabBar.BackgroundColor = UIColor.White;
                tabBar.Translucent = false;

                // Create top border
                var borderView = new UIView
                {
                    BackgroundColor = UIColor.FromRGB(208, 208, 208)
                };

                tabBar.AddSubview(borderView);
                borderView.TranslatesAutoresizingMaskIntoConstraints = false;

                NSLayoutConstraint.ActivateConstraints(new[]
                {
                    borderView.TopAnchor.ConstraintEqualTo(tabBar.TopAnchor),
                    borderView.LeadingAnchor.ConstraintEqualTo(tabBar.LeadingAnchor),
                    borderView.TrailingAnchor.ConstraintEqualTo(tabBar.TrailingAnchor),
                    borderView.HeightAnchor.ConstraintEqualTo(2)
                });

                // Add shadow
                tabBar.Layer.ShadowOpacity = 0.3f;
                tabBar.Layer.ShadowRadius = 8;
                tabBar.Layer.ShadowOffset = new CGSize(0, -4);
                tabBar.Layer.ShadowColor = UIColor.Black.CGColor;
                tabBar.Layer.MasksToBounds = false;

                // Customize selected tab appearance
                CustomizeSelectedTabAppearance(tabBar);
                return;
            }

            for (int i = 0; i < view.Subviews.Length; i++)
            {
                FindAndStyleTabBar(view.Subviews[i]);
            }
#endif
        }

        private static void CustomizeSelectedTabAppearance(UITabBar tabBar)
        {
#if IOS
            try
            {
                // Set the selected tab item background color (red: #FF0000)
                var selectedColor = UIColor.FromRGB(255, 0, 0);

                // For iOS 13+, use appearance API
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    var appearance = new UITabBarAppearance();
                    appearance.ConfigureWithDefaultBackground();
                    appearance.BackgroundColor = UIColor.White;

                    // Selected item appearance
                    var selectedItemAppearance = new UITabBarItemAppearance();
                    selectedItemAppearance.Selected.IconColor = selectedColor;
                    selectedItemAppearance.Selected.TitleTextAttributes = new UIStringAttributes { ForegroundColor = selectedColor };

                    // Unselected item appearance
                    var normalItemAppearance = new UITabBarItemAppearance();
                    normalItemAppearance.Normal.IconColor = UIColor.FromRGB(64, 64, 64);
                    normalItemAppearance.Normal.TitleTextAttributes = new UIStringAttributes { ForegroundColor = UIColor.FromRGB(64, 64, 64) };

                    appearance.StackedLayoutAppearance = selectedItemAppearance;
                    appearance.InlineLayoutAppearance = selectedItemAppearance;
                    appearance.CompactInlineLayoutAppearance = selectedItemAppearance;

                    tabBar.StandardAppearance = appearance;
                    if (UIDevice.CurrentDevice.CheckSystemVersion(15, 0))
                    {
                        tabBar.ScrollEdgeAppearance = appearance;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error customizing iOS selected tab: {ex.Message}");
            }
#endif
        }
    }
}
