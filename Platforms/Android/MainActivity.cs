using AirIQ.Platforms.Android;
using AirIQ.Views;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;


namespace AirIQ;

[Activity(Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize |
        ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private global::Android.Views.View? _statusBarScrim;
    private global::Android.Graphics.Color? _lastAppliedStatusBarScrimColor;
    private global::Android.Graphics.Color? _lastAppliedChromeColor;
    private bool _windowInsetsListenerInstalled;
    private int _lastAppliedStatusBarInset = -1;
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        EnsureWindowInsetsListener();
        ApplySystemBars();
    }

    protected override void OnResume()
    {
        base.OnResume();
        EnsureWindowInsetsListener();
        ApplySystemBars();
    }

    public override void OnWindowFocusChanged(bool hasFocus)
    {
        base.OnWindowFocusChanged(hasFocus);

        if (hasFocus)
            ApplySystemBars();
    }

    private void EnsureWindowInsetsListener()
    {
        if (_windowInsetsListenerInstalled || Window?.DecorView is null)
            return;

        ViewCompat.SetOnApplyWindowInsetsListener(Window.DecorView, new WindowInsetsListener(this));
        ViewCompat.RequestApplyInsets(Window.DecorView);
        _windowInsetsListenerInstalled = true;
    }

    private void OnWindowInsetsChanged()
    {
        var currentPage = Shell.Current?.CurrentPage;
        var statusBarInset = GetStatusBarInset();

        if (_lastAppliedStatusBarInset != statusBarInset || _lastAppliedChromeColor is null)
            ApplySystemBars(currentPage);
    }

    public void ApplySystemBars(Page? page = null)
    {
        if (Window?.DecorView is null)
            return;

        WindowCompat.SetDecorFitsSystemWindows(Window, false);
        Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
        Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
        Window.ClearFlags(WindowManagerFlags.TranslucentNavigation);

#pragma warning disable CA1422
        Window.SetStatusBarColor(global::Android.Graphics.Color.Transparent);
        Window.SetNavigationBarColor(global::Android.Graphics.Color.Transparent);
#pragma warning restore CA1422

        var resolvedPage = page ?? Shell.Current?.CurrentPage;
        var chromeColor = ResolveChromeColor();
        var controller = WindowCompat.GetInsetsController(Window, Window.DecorView);
        if (controller is not null)
        {
            var useDarkIcons = IsLightColor(chromeColor);
            controller.AppearanceLightStatusBars = useDarkIcons;
            controller.AppearanceLightNavigationBars = useDarkIcons;
        }

        if (resolvedPage?.GetType() == typeof(DashboardPage))
        {
            RemoveStatusBarScrim();
        }
        else
        {
            ApplyStatusBarScrim(chromeColor);
        }

        _lastAppliedChromeColor = chromeColor;
    }

    private void RemoveStatusBarScrim()
    {
        if (_statusBarScrim?.Parent is ViewGroup parent)
        {
            parent.RemoveView(_statusBarScrim);
        }

        _statusBarScrim = null;
        _lastAppliedStatusBarScrimColor = null;
    }

    private void ApplyStatusBarScrim(global::Android.Graphics.Color chromeColor)
    {
        if (Window?.DecorView is not ViewGroup decorRoot)
            return;


        var statusBarInset = GetStatusBarInset();
        _lastAppliedStatusBarInset = statusBarInset;
        if (statusBarInset <= 0)
            return;

        if (_statusBarScrim is null || _statusBarScrim.Parent is null)
        {
            _statusBarScrim = new global::Android.Views.View(this)
            {
                Clickable = false,
                Focusable = false
            };

            var layoutParams = new global::Android.Widget.FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                statusBarInset)
            {
                Gravity = GravityFlags.Top
            };

            decorRoot.AddView(_statusBarScrim, decorRoot.ChildCount, layoutParams);
        }


        if (_statusBarScrim.LayoutParameters is global::Android.Widget.FrameLayout.LayoutParams frameLayoutParams)
        {
            if (frameLayoutParams.Height != statusBarInset)
            {
                frameLayoutParams.Height = statusBarInset;
                _statusBarScrim.LayoutParameters = frameLayoutParams;
            }
        }

        if (_lastAppliedStatusBarScrimColor is null || !_lastAppliedStatusBarScrimColor.Equals(chromeColor) || _statusBarScrim.Background is null)
        {

            _statusBarScrim.SetBackgroundColor(chromeColor);
            _lastAppliedStatusBarScrimColor = chromeColor;
        }

        _statusBarScrim.BringToFront();
    }

    private int GetStatusBarInset()
    {
        var decorView = Window?.DecorView;
        if (decorView is null)
            return 0;

        var insets = ViewCompat.GetRootWindowInsets(decorView);
        if (insets is not null)
        {
            var statusBarInsets = insets.GetInsets(WindowInsetsCompat.Type.StatusBars());
            var topInset = statusBarInsets?.Top ?? 0;
            if (topInset > 0)
                return topInset;
        }

        var resourceId = Resources?.GetIdentifier("status_bar_height", "dimen", "android") ?? 0;
        return resourceId > 0 ? Resources?.GetDimensionPixelSize(resourceId) ?? 0 : 0;
    }

    private static global::Android.Graphics.Color ResolveChromeColor()
    {
        var color = (Color)(App.Current?.Resources["PrimaryColor"] ?? Colors.Black);
        return color.ToPlatform();
    }

    private static bool IsLightColor(global::Android.Graphics.Color color)
    {
        if (color.A == 0)
            return false;

        var luminance = ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255d;
        return luminance >= 0.6d;
    }

    private static AndroidX.DrawerLayout.Widget.DrawerLayout? FindDrawerLayout(global::Android.Views.View? view)
    {
        if (view is null)
            return null;

        if (view is AndroidX.DrawerLayout.Widget.DrawerLayout drawerLayout)
            return drawerLayout;

        if (view is ViewGroup group)
        {
            for (var i = 0; i < group.ChildCount; i++)
            {
                var match = FindDrawerLayout(group.GetChildAt(i));
                if (match is not null)
                    return match;
            }
        }

        return null;
    }

    private sealed class WindowInsetsListener : Java.Lang.Object, AndroidX.Core.View.IOnApplyWindowInsetsListener
    {
        private readonly MainActivity _activity;

        public WindowInsetsListener(MainActivity activity)
        {
            _activity = activity;
        }

        public WindowInsetsCompat? OnApplyWindowInsets(global::Android.Views.View? v, WindowInsetsCompat? insets)
        {
            _activity.OnWindowInsetsChanged();
            return insets;
        }
    }
}