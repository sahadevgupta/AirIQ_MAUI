using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using NavigationPage = Microsoft.Maui.Controls.NavigationPage;

namespace AirIQ.Views;

public abstract class BasePage : ContentPage
{
	public BasePage()
	{
		Shell.SetNavBarIsVisible(this, false);
		NavigationPage.SetHasNavigationBar(this, false);
		ApplyStatusBarStyle();
	}
	
	#region [ Methods ]

	private void ApplyStatusBarStyle()
	{
		this.Behaviors.Add(new StatusBarBehavior
		{
			StatusBarColor = Color.FromArgb("#2E6FB6"),
			StatusBarStyle = StatusBarStyle.LightContent
		});

		if (OperatingSystem.IsIOS())
		{
			var safeInsects = On<iOS>().SafeAreaInsets();
			if (safeInsects.Top <= 0)
			{
				On<iOS>().SetUseSafeArea(true);
			}
			else
			{
				this.Padding = new Thickness(0, safeInsects.Top, 0, 0);
			}
		}
	}
    #endregion
}