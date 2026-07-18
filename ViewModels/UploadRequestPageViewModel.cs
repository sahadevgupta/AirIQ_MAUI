using AirIQ.Models.Request;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class UploadRequestPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService) : BaseViewModel(viewModelParameters)
    {
        #region [ Properties ]

        [ObservableProperty]
        private decimal? _amount;

        [ObservableProperty]
        private string? _referenceNumber;

        [ObservableProperty]
        private string? _selectedPaymentMode;

        [ObservableProperty]
        private string? _filePath;

        [ObservableProperty]
        private string _fileName = "No file chosen";

        [ObservableProperty]
        private string? _message;
        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task PickFileAsync()
        {
            var fileResult = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select File"
            });

            if (fileResult is not null)
            {
                FilePath = fileResult.FullPath;
                FileName = fileResult.FileName;
            }
        }

        [RelayCommand]
        private async Task SubmitAsync()
        {
            var request = new UploadRequest
            {
                Amount = Amount.GetValueOrDefault(),
                FilePath = FilePath,
                Message = Message,
                PaymentMode = SelectedPaymentMode,
                RefNumber = ReferenceNumber
            };

            await operationsService.UploadRequestAsync(request);
        }

        #endregion
    }
}