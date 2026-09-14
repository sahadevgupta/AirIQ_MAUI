using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
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
                // Set light blue background
                UIColor barBackgroundColor;
                try
                {
                    barBackgroundColor = ((Color)Application.Current.Resources["Primary0"]).ToPlatform();
                }
                catch
                {
                    barBackgroundColor = UIColor.White;
                }
                tabBar.BarTintColor = barBackgroundColor;
                tabBar.BackgroundColor = barBackgroundColor;
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

                // Pop the active tab's navigation stack to root whenever a tab item is tapped
                // (fires even when re-tapping the already-selected tab, unlike Shell's own Navigated event)
                tabBar.ItemSelected -= OnTabItemSelected;
                tabBar.ItemSelected += OnTabItemSelected;
                return;
            }

            for (int i = 0; i < view.Subviews.Length; i++)
            {
                FindAndStyleTabBar(view.Subviews[i]);
            }
#endif
        }

        private static void OnTabItemSelected(object? sender, UITabBarItemEventArgs e)
        {
            PopCurrentTabToRootAsync();
        }

        private static async void PopCurrentTabToRootAsync()
        {
            try
            {
                var selectedSection = Shell.Current?.CurrentItem?.CurrentItem;
                if (selectedSection?.Navigation?.NavigationStack?.Count > 1)
                {
                    await selectedSection.Navigation.PopToRootAsync(animated: false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error popping current tab to root: {ex.Message}");
            }
        }

        private static void CustomizeSelectedTabAppearance(UITabBar tabBar)
        {
#if IOS
            try
            {
                // Use the app's PrimaryRed theme color for the selected tab icon/title, matching Android
                UIColor selectedColor;
                try
                {
                    selectedColor = ((Color)Application.Current.Resources["PrimaryRed"]).ToPlatform();
                }
                catch
                {
                    selectedColor = UIColor.FromRGB(241, 99, 103); // #F16367 fallback, matches PrimaryRed
                }

                // For iOS 13+, use appearance API
                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    UIColor barBackgroundColor;
                    try
                    {
                        barBackgroundColor = ((Color)Application.Current.Resources["Primary0"]).ToPlatform();
                    }
                    catch
                    {
                        barBackgroundColor = UIColor.White;
                    }

                    var appearance = new UITabBarAppearance();
                    appearance.ConfigureWithDefaultBackground();
                    appearance.BackgroundColor = barBackgroundColor;

                    // Selected item appearance (icon stays white, title keeps the theme color)
                    var selectedItemAppearance = new UITabBarItemAppearance();
                    selectedItemAppearance.Selected.IconColor = UIColor.White;
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
