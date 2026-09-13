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
using Google.Android.Material.Shape;

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

                        // Set light blue background
                        try
                        {
                            bottomNav.SetBackgroundColor(Microsoft.Maui.Graphics.Color.FromArgb("#ffffff").ToPlatform());
                        }
                        catch
                        {
                            bottomNav.SetBackgroundColor(Color.White);
                        }

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

                        // Pop the active tab's navigation stack to root when its already-selected icon is tapped again
                        AttachTabReselectHandler(bottomNav);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error customizing TabBar: {ex.Message}");
            }

        }

        private static void AttachTabReselectHandler(ViewGroup bottomNav)
        {
            try
            {
                if (bottomNav is BottomNavigationView bnv)
                {
                    bnv.SetOnItemReselectedListener(new ItemReselectedListener(PopCurrentTabToRootAsync));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error attaching tab reselect handler: {ex.Message}");
            }
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

        private sealed class ItemReselectedListener : Java.Lang.Object, Google.Android.Material.Navigation.NavigationBarView.IOnItemReselectedListener
        {
            private readonly Action _onReselected;

            public ItemReselectedListener(Action onReselected) => _onReselected = onReselected;

            public void OnNavigationItemReselected(IMenuItem item) => _onReselected();
        }

        private static void CustomizeSelectedTabBackground(ViewGroup bottomNav)
        {

            try
            {
                // Access the BottomNavigationView and customize the selected item appearance
                if (bottomNav is BottomNavigationView bnv)
                {
                    // Give the active indicator a square-ish shape (10dp corner radius) instead of the default pill.
                    // Kept independent of the color lookup below so a failure there can't block the shape change.
                    try
                    {
                        float density = bnv.Context?.Resources?.DisplayMetrics?.Density ?? 1f;
                        float cornerRadiusPx = 10 * density;
                        var shapeAppearance = new ShapeAppearanceModel.Builder()
                            .SetAllCornerSizes(cornerRadiusPx)
                            .Build();
                        bnv.ItemActiveIndicatorShapeAppearance = shapeAppearance;
                    }
                    catch
                    {
                        // Fallback if property not available in this version
                    }

                    // Note: the resource is a Microsoft.Maui.Graphics.Color; ToPlatform() converts it to
                    // Android.Graphics.Color (this file aliases "Color" to the Android type above).
                    Color selectedColor = ((Microsoft.Maui.Graphics.Color)Application.Current.Resources["Primary10"]).ToPlatform();

                    // Set active indicator color (selected item background) - for Material Design 3
                    try
                    {
                        bnv.ItemActiveIndicatorColor = ColorStateList.ValueOf(selectedColor);
                    }
                    catch
                    {
                        // Fallback if property not available in this version
                    }

                    // Set the ripple color for selection feedback
                    bnv.ItemRippleColor = ColorStateList.ValueOf(selectedColor);

                    // Set the selected icon color to white, independent of the title text color
                    // try
                    // {
                    //     Color unselectedIconColor = ((Microsoft.Maui.Graphics.Color)Application.Current.Resources["Gray600"]).ToPlatform();
                    //     var iconStates = new int[][]
                    //     {
                    //         new int[] { global::Android.Resource.Attribute.StateChecked },
                    //         new int[] { }
                    //     };
                    //     var iconColors = new int[] { Color.White.ToArgb(), unselectedIconColor.ToArgb() };
                    //     bnv.ItemIconTintList = new ColorStateList(iconStates, iconColors);
                    // }
                    // catch
                    // {
                    //     // Fallback if property not available in this version
                    // }
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
