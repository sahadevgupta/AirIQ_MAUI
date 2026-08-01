using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;
using Android.Graphics;
using Android.Widget;
using Android.Views;
using Android.Content.Res;
using Android.Util;
using Paint = Android.Graphics.Paint;
using Color = Android.Graphics.Color;
using View = Android.Views.View;
using Google.Android.Material.BottomNavigation;

namespace AirIQ.Platforms.Handlers
{
    public static class TabBarCustomization
    {
        public static void CustomizeTabBar(Shell shell)
        {

            shell.Loaded += (s, e) =>
            {
                AddTabBarBorder(shell);
            };

        }

        private static void AddTabBarBorder(Shell shell)
        {

            try
            {
                var handler = shell.Handler as IElementHandler;
                if (handler?.PlatformView is ViewGroup platformView)
                {
                    // Find the bottom navigation view
                    var bottomNav = FindBottomNavigationView(platformView);
                    if (bottomNav != null)
                    {
                        // Set high elevation for prominent shadow
                        bottomNav.Elevation = 16;

                        // Set white background
                        bottomNav.SetBackgroundColor(Color.White);

                        // Add top border line
                        var borderView = new View(bottomNav.Context)
                        {
                            LayoutParameters = new ViewGroup.LayoutParams(
                                ViewGroup.LayoutParams.MatchParent,
                                (int)(2 * (bottomNav.Context?.Resources?.DisplayMetrics?.Density ?? 1)))
                        };
                        borderView.SetBackgroundColor(Color.ParseColor("#D0D0D0"));

                        if (bottomNav is ViewGroup viewGroup)
                        {
                            viewGroup.AddView(borderView, 0);
                        }

                        // Customize selected tab item background
                        CustomizeSelectedTabBackground(bottomNav);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error customizing TabBar: {ex.Message}");
            }

        }

        private static void CustomizeSelectedTabBackground(ViewGroup bottomNav)
        {

            try
            {
                // Access the BottomNavigationView and customize the selected item appearance
                if (bottomNav is BottomNavigationView bnv)
                {
                    int selectedColor = Color.ParseColor("#BFD4FC");

                    // Set active indicator color (selected item background) - for Material Design 3
                    try
                    {
                        bnv.ItemActiveIndicatorColor = ColorStateList.ValueOf(new Color(selectedColor));
                    }
                    catch
                    {
                        // Fallback if property not available in this version
                    }

                    // Set the ripple color for selection feedback
                    bnv.ItemRippleColor = ColorStateList.ValueOf(new Color(selectedColor));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error customizing selected tab: {ex.Message}");
            }

        }

        private static ViewGroup? FindBottomNavigationView(ViewGroup viewGroup)
        {
            for (int i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);
                var className = child?.GetType().Name ?? "";

                if (className.Contains("BottomNavigationView") || className.Contains("NavigationBar") || className.Contains("BottomAppBar"))
                {
                    return child as ViewGroup;
                }

                if (child is ViewGroup childGroup)
                {
                    var result = FindBottomNavigationView(childGroup);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }
    }
}
