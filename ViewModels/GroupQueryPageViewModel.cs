using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Configurations;
using AirIQ.Configurations.Mapper;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    public partial class GroupQueryPageViewModel : BaseViewModel
    {
        #region [ Properties ]

        const int pageSize = 20;

        private readonly IOperationsService operationsService;

        private List<GroupQuery> groupQueryRecordsTemp = new();

        [ObservableProperty]
        private ObservableCollection<TabModel>? _tabs;

        [ObservableProperty]
        private TabModel? _selectedTab;

        // Details tab - request form fields.
        [ObservableProperty]
        private string? _sectorOnward;

        [ObservableProperty]
        private string? _travelDate;

        [ObservableProperty]
        private string? _sectorReturn;

        [ObservableProperty]
        private string? _returnTravelDate;

        [ObservableProperty]
        private string? _noOfPax;

        [ObservableProperty]
        private string? _expectedFare;

        [ObservableProperty]
        private string? _preferredAirline;

        [ObservableProperty]
        private string? _contactNo;

        [ObservableProperty]
        private string? _message;

        // History tab - filters and results. ExtendedDatePicker.Date is a non-nullable DateTime
        // whose own default(DateTime) value is its "unset" sentinel (see ExtendedDatePicker.xaml.cs),
        // so these follow the same convention rather than using DateTime?.
        [ObservableProperty]
        private DateTime _historyFromDate;

        [ObservableProperty]
        private DateTime _historyToDate;

        [ObservableProperty]
        private ObservableCollection<string> _tripTypes = new() { AppResource.OneWay, AppResource.RoundTrip };

        [ObservableProperty]
        private string? _selectedTripType;

        [ObservableProperty]
        private ObservableCollection<string> _statuses = new();

        [ObservableProperty]
        private string? _selectedStatus;

        [ObservableProperty]
        private ObservableCollection<GroupQuery> _groupQueryRecords = new();

        #endregion

        public GroupQueryPageViewModel(IViewModelParameters viewModelParameters, IOperationsService operationsService) : base(viewModelParameters)
        {
            this.operationsService = operationsService;
        }

        #region [ Methods & Service Calls ]

        private void InitData()
        {
            Tabs = new ObservableCollection<TabModel>
            {
                new TabModel{ Name = AppResource.DetailsTab, Icon ="detail_icon", IsSelected=true},
                new TabModel{ Name = AppResource.HistoryTab, Icon="history"}
            };
        }

        private void FilterGroupQueryHistory()
        {
            IEnumerable<GroupQuery> filtered = groupQueryRecordsTemp;

            if (!string.IsNullOrWhiteSpace(SelectedTripType))
                filtered = filtered.Where(x => x.TicketType?.Contains(SelectedTripType, StringComparison.OrdinalIgnoreCase) == true);

            if (!string.IsNullOrWhiteSpace(SelectedStatus))
                filtered = filtered.Where(x => string.Equals(x.Status, SelectedStatus, StringComparison.OrdinalIgnoreCase));

            if (HistoryFromDate != default)
                filtered = filtered.Where(x => DateTime.TryParse(x.RequestDate, out var requestDate) && requestDate.Date >= HistoryFromDate.Date);

            if (HistoryToDate != default)
                filtered = filtered.Where(x => DateTime.TryParse(x.RequestDate, out var requestDate) && requestDate.Date <= HistoryToDate.Date);

            GroupQueryRecords = new ObservableCollection<GroupQuery>(filtered);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private void TabItemSelected(object? item)
        {
            TabModel tab = (TabModel)item!;
            var previousSelectedItem = Tabs?.FirstOrDefault(x => x.IsSelected);
            previousSelectedItem?.IsSelected = false;

            //var selectedItem = Tabs?.FirstOrDefault(t => t.Name == tabName);
            tab?.IsSelected = true;
        }

        // No submit/create API exists for group queries yet (only GetGroupQueryRecordsAsync, a
        // read-only fetch, is available - see IOperationsService/IAppBackendService). Rather than
        // fabricate a booking-confirmation flow that doesn't exist server-side, this surfaces the
        // same "not implemented yet" messaging already used elsewhere in the app (see
        // DashboardPage2ViewModel.ViewAllDestinations) until a real submit endpoint is added.
        [RelayCommand]
        private void ConfirmBooking()
        {
            ShowToast(AppResource.FeatureComingSoon);
        }

        [RelayCommand]
        private async Task SearchGroupQueryHistoryAsync()
        {
            try
            {
                using (LoadingService.Show())
                {
                    var records = await operationsService.GetGroupQueryRecordsAsync(AppConfiguration.CurrentUser?.AgencyId ?? 0, 1, pageSize);
                    groupQueryRecordsTemp = BackendToAppModelMapper.GetGroupQueryRecords(records).ToList();

                    Statuses = new ObservableCollection<string>(groupQueryRecordsTemp
                        .Where(x => !string.IsNullOrWhiteSpace(x.Status))
                        .Select(x => x.Status!)
                        .Distinct());
                }

                FilterGroupQueryHistory();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        #endregion

        #region [ Override Method ]

        public override Task LoadDataWhenNavigatedTo(CancellationToken cancellationToken = default)
        {
            InitData();
            return base.LoadDataWhenNavigatedTo();
        }

        #endregion

    }
}