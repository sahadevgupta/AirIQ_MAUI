using System;
using CommunityToolkit.Maui.Core;

namespace AirIQ.Services.Interfaces;

public interface IDialogService
{
    Task ShowStatusAlertAsync(string message, bool response = true, int timeout = 2500);
    void ShowToast(string message, double toastfontSize = 14, ToastDuration toastDuration = ToastDuration.Short);
    Task ShowSnackBarAync(string message, bool isSuccess, double fontSize, double duration, string? actionText, Action? action);
}
