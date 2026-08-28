using System.Windows.Input;
using Microsoft.Maui.Platform;
using ContentView = Microsoft.Maui.Controls.ContentView;

namespace AirIQ.Controls;

public partial class SearchView : ContentView
{
	/// <summary>
	///     Identifies the <see cref="SearchText" /> bindable property.
	/// </summary>
	public static readonly BindableProperty SearchTextProperty =
		BindableProperty.Create(nameof(SearchText), typeof(string), typeof(SearchView), defaultBindingMode: BindingMode.TwoWay);

	/// <summary>
	///     Identifies the <see cref="PlaceholderText" /> bindable property.
	/// </summary>
	public static readonly BindableProperty PlaceholderTextProperty =
		BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(SearchView), defaultValue: "Search here..");

	/// <summary>
	///     Identifies the <see cref="SearchCommand" /> bindable property.
	/// </summary>
	public static readonly BindableProperty SearchCommandProperty =
		BindableProperty.Create(nameof(SearchCommand), typeof(ICommand), typeof(SearchView), default(ICommand));

	/// <summary>
	///     Identifies the <see cref="FilterCommand" /> bindable property.
	/// </summary>
	public static readonly BindableProperty FilterCommandProperty =
		BindableProperty.Create(nameof(FilterCommand), typeof(ICommand), typeof(SearchView), default(ICommand));

	/// <summary>
	///     Identifies the <see cref="DownloadCommand" /> bindable property.
	/// </summary>
	public static readonly BindableProperty DownloadCommandProperty =
		BindableProperty.Create(nameof(DownloadCommand), typeof(ICommand), typeof(SearchView), default(ICommand));

	/// <summary>
	///     Identifies the <see cref="IsDownloadVisible" /> bindable property.
	/// </summary>
	public static readonly BindableProperty IsDownloadVisibleProperty =
		BindableProperty.Create(nameof(IsDownloadVisible), typeof(bool), typeof(SearchView), true);

	/// <summary>
	///     Identifies the <see cref="DownloadCommand" /> bindable property.
	/// </summary>
	public static readonly BindableProperty IsFilterVisibleProperty =
		BindableProperty.Create(nameof(IsFilterVisible), typeof(bool), typeof(SearchView), true);

	public string SearchText
	{
		get => (string)GetValue(SearchTextProperty);
		set => SetValue(SearchTextProperty, value);
	}

	public string PlaceholderText
	{
		get => (string)GetValue(PlaceholderTextProperty);
		set => SetValue(PlaceholderTextProperty, value);
	}

	public ICommand SearchCommand
	{
		get => (ICommand)GetValue(SearchCommandProperty);
		set => SetValue(SearchCommandProperty, value);
	}

	public ICommand FilterCommand
	{
		get => (ICommand)GetValue(FilterCommandProperty);
		set => SetValue(FilterCommandProperty, value);
	}

	public ICommand DownloadCommand
	{
		get => (ICommand)GetValue(DownloadCommandProperty);
		set => SetValue(DownloadCommandProperty, value);
	}

	public bool IsDownloadVisible
	{
		get => (bool)GetValue(IsDownloadVisibleProperty);
		set => SetValue(IsDownloadVisibleProperty, value);
	}

	public bool IsFilterVisible
	{
		get => (bool)GetValue(IsFilterVisibleProperty);
		set => SetValue(IsFilterVisibleProperty, value);
	}


	public SearchView()
	{
		InitializeComponent();
		ModifyControl();
	}

	private void ModifyControl()
	{
		Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping(nameof(SearchView), (handler, view) =>
		{
#if IOS
			handler.PlatformView.BackgroundColor = Colors.Transparent.ToPlatform();
			//handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
			handler.PlatformView.ClipsToBounds = true;
			handler.PlatformView.Layer.BorderWidth = 0;
			handler.PlatformView.Layer.BorderColor = UIKit.UIColor.Clear.CGColor;
#elif ANDROID

			handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
			handler.PlatformView.Background = null;

#endif
		});
	}
}