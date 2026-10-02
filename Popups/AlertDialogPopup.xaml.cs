using System.ComponentModel;
using AirIQ.Extensions;
using Mopups.Interfaces;
using Mopups.Pages;
using Mopups.Services;

namespace AirIQ.Popups;

public partial class AlertDialogPopup : PopupPage
{
	// Serializes calls so only one alert popup is ever on screen at a time; concurrent
	// callers queue behind it instead of stacking overlapping popups.
	private static readonly SemaphoreSlim DisplaySemaphore = new(1, 1);

	public static readonly BindableProperty HeaderTitleProperty =
		BindableProperty.Create(nameof(HeaderTitle), typeof(string), typeof(AlertDialogPopup), null, BindingMode.TwoWay);

	public static readonly BindableProperty MessageProperty =
		BindableProperty.Create(nameof(Message), typeof(string), typeof(AlertDialogPopup), null, BindingMode.TwoWay);

	public static readonly BindableProperty AcceptTextProperty =
		BindableProperty.Create(nameof(AcceptText), typeof(string), typeof(AlertDialogPopup), null, BindingMode.TwoWay);

	public static readonly BindableProperty CancelTextProperty =
		BindableProperty.Create(nameof(CancelText), typeof(string), typeof(AlertDialogPopup), null, BindingMode.TwoWay);

	public static readonly BindableProperty IsCancelVisibleProperty =
		BindableProperty.Create(nameof(IsCancelVisible), typeof(bool), typeof(AlertDialogPopup), false);

	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(nameof(Icon), typeof(string), typeof(AlertDialogPopup), FontAwesomeIcons.InfoCircle, BindingMode.TwoWay);

	public static readonly BindableProperty IconTintColorProperty =
		BindableProperty.Create(nameof(IconTintColor), typeof(Color), typeof(AlertDialogPopup), default(Color));

	public string HeaderTitle
	{
		get => (string)GetValue(HeaderTitleProperty);
		set => SetValue(HeaderTitleProperty, value);
	}

	public string Message
	{
		get => (string)GetValue(MessageProperty);
		set => SetValue(MessageProperty, value);
	}

	public string AcceptText
	{
		get => (string)GetValue(AcceptTextProperty);
		set => SetValue(AcceptTextProperty, value);
	}

	public string? CancelText
	{
		get => (string?)GetValue(CancelTextProperty);
		set => SetValue(CancelTextProperty, value);
	}

	public bool IsCancelVisible
	{
		get => (bool)GetValue(IsCancelVisibleProperty);
		set => SetValue(IsCancelVisibleProperty, value);
	}

	[TypeConverter(typeof(ImageSource))]
	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public Color IconTintColor
	{
		get => (Color)GetValue(IconTintColorProperty);
		set => SetValue(IconTintColorProperty, value);
	}

	private readonly TaskCompletionSource<bool> _resultCompletionSource = new();
	private int _resultSet;

	public AlertDialogPopup()
	{
		InitializeComponent();
	}

	private async void OnAcceptClicked(object sender, EventArgs e) => await CompleteAsync(true);

	private async void OnCancelClicked(object sender, EventArgs e) => await CompleteAsync(false);

	private async Task CompleteAsync(bool result)
	{
		// Interlocked guard: a rapid double-tap can queue a second click event before the
		// first has disabled the buttons, so the flag - not IsEnabled - is the source of truth.
		if (Interlocked.Exchange(ref _resultSet, 1) != 0)
		{
			return;
		}

		AcceptButton.IsEnabled = false;
		CancelButton.IsEnabled = false;

		try
		{
			await MopupService.Instance.PopAsync();
		}
		finally
		{
			_resultCompletionSource.TrySetResult(result);
		}
	}

	/// <summary>
	/// Pushes the themed modal alert and awaits the user's selection.
	/// </summary>
	public static async Task<bool> ShowAsync(
		IPopupNavigation popupNavigation,
		string title,
		string message,
		string acceptText,
		string? cancelText,
		string icon,
		Color iconTintColor)
	{
		await DisplaySemaphore.WaitAsync();
		try
		{
			var popup = new AlertDialogPopup
			{
				HeaderTitle = title,
				Message = message,
				AcceptText = acceptText,
				CancelText = cancelText,
				Icon = icon,
				IconTintColor = iconTintColor,
				IsCancelVisible = !string.IsNullOrWhiteSpace(cancelText)
			};

			await MainThread.InvokeOnMainThreadAsync(() => popupNavigation.PushAsync(popup));

			return await popup._resultCompletionSource.Task;
		}
		finally
		{
			DisplaySemaphore.Release();
		}
	}
}
