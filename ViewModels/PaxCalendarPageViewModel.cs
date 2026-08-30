using System.Collections.ObjectModel;
using AirIQ.Configurations;
using AirIQ.Constants;
using AirIQ.Extensions;
using AirIQ.Models;
using AirIQ.Models.Response;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class PaxCalendarPageViewModel(IViewModelParameters viewModelParameters,
        IOperationsService operationsService) : BaseViewModel(viewModelParameters)
    {
        private const int VisibleWindowSize = 3;
        private DateTime _windowStartDate = DateTime.Today.AddDays(-1);

        #region [ Properties ]

        [ObservableProperty]
        private DateTime _currentDate = DateTime.Today;

        [ObservableProperty]
        private DateTime _selectedTravelDate;

        private List<PaxCalendarFlightDto> flightBookingsTemp = new();

        [ObservableProperty]
        private ObservableCollection<PaxCalendarFlightDto>? _flightBookingList;

        [ObservableProperty]
        private string? _searchText;

        private DateCardItem? _leftDateItem;

        private DateCardItem? _centerDateItem;

        private DateCardItem? _rightDateItem;

        [ObservableProperty]
        private bool _isCcBooking;

        [ObservableProperty]
        private DateCardItem? _selectedDateItem;

        public DateCardItem? LeftDateItem
        {
            get => _leftDateItem;
            set => SetProperty(ref _leftDateItem, value);
        }

        public DateCardItem? CenterDateItem
        {
            get => _centerDateItem;
            set => SetProperty(ref _centerDateItem, value);
        }

        public DateCardItem? RightDateItem
        {
            get => _rightDateItem;
            set => SetProperty(ref _rightDateItem, value);
        }

        [ObservableProperty]
        private ObservableCollection<DateCardItem>? _dateItems;

        #endregion

        #region [ Methods & Service Call ]

        private async Task InitializeDataAsync()
        {
            _windowStartDate = DateTime.Today.AddDays(-1);
            BuildDateWindow(DateTime.Today);
            IsCcBooking = false;
            await Task.CompletedTask;
        }

        private void UpdateSelectedState()
        {
            var items = new[] { LeftDateItem, CenterDateItem, RightDateItem };
            foreach (var item in items)
            {
                if (item is null)
                    continue;

                item.IsSelected = ReferenceEquals(item, SelectedDateItem);
            }
        }

        private void BuildDateWindow(DateTime selectedDate)
        {
            LeftDateItem = new DateCardItem(_windowStartDate, AppResource.TicketPaxSummaryPlaceholder);
            CenterDateItem = new DateCardItem(_windowStartDate.AddDays(1), AppResource.TicketPaxSummaryPlaceholder);
            RightDateItem = new DateCardItem(_windowStartDate.AddDays(2), AppResource.TicketPaxSummaryPlaceholder);

            SelectedDateItem = selectedDate.Date == LeftDateItem.Date.Date
                ? LeftDateItem
                : selectedDate.Date == RightDateItem.Date.Date
                    ? RightDateItem
                    : CenterDateItem;

            DateItems?.Clear();
            DateItems = new ObservableCollection<DateCardItem>
            {
                LeftDateItem,
                CenterDateItem,
                RightDateItem
            };
        }

        private async Task LoadFlightBookingsAsync(DateTime travelDate)
        {
            using (LoadingService.Show())
            {
                var result = await operationsService.GetPaxCalendarFlightAsync(
                    AppConfiguration.CurrentUser?.AgencyId ?? 0,
"2026-09-01");
                // travelDate.ToString("yyyy-MM-dd"));

                flightBookingsTemp = result?.ToList() ?? new List<PaxCalendarFlightDto>();
                FilterFlightBookings(SearchText);
            }
        }

        private void FilterFlightBookings(string? searchKey)
        {
            var filtered = string.IsNullOrEmpty(searchKey)
                ? flightBookingsTemp
                : flightBookingsTemp.Where(x =>
                    x.TicketRefNo.ContainsIgnoreCase(searchKey) ||
                    x.PNR.ContainsIgnoreCase(searchKey) ||
                    x.SourceCity.ContainsIgnoreCase(searchKey) ||
                    x.DestinationCity.ContainsIgnoreCase(searchKey) ||
                    x.AirlineCode.ContainsIgnoreCase(searchKey) ||
                    x.FlightNumber.ContainsIgnoreCase(searchKey));

            FlightBookingList = new ObservableCollection<PaxCalendarFlightDto>(filtered);
        }

        partial void OnSearchTextChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
                FilterFlightBookings(value);
        }

        partial void OnSelectedDateItemChanged(DateCardItem? value)
        {
            UpdateSelectedState();

            if (value is null)
                return;

            SelectedTravelDate = value.Date;
            CurrentDate = value.Date;
            _ = LoadFlightBookingsAsync(value.Date);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private void PreviousDate()
        {
            _windowStartDate = _windowStartDate.AddDays(-1);
            BuildDateWindow(_windowStartDate.AddDays(VisibleWindowSize / 2));
        }

        [RelayCommand]
        private void NextDate()
        {
            _windowStartDate = _windowStartDate.AddDays(1);
            BuildDateWindow(_windowStartDate.AddDays(VisibleWindowSize / 2));
        }

        [RelayCommand]
        private void SelectDate(DateCardItem? item)
        {
            if (item is null)
                return;

            SelectedDateItem = item;
        }

        [RelayCommand]
        private void Search(string? searchText)
        {
            FilterFlightBookings(searchText);
        }

        #endregion

        #region [ Override Methods ]
        public override async Task LoadDataWhenNavigatedTo()
        {
            await InitializeDataAsync();
        }

        #endregion
    }
}