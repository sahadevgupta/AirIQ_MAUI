using System.Collections.ObjectModel;
using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Extensions;
using AirIQ.Models;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels
{
    [QueryProperty(nameof(FieldType), NavigationParamConstants.AirportFieldType)]
    [QueryProperty(nameof(Airports), NavigationParamConstants.AirportList)]
    public partial class AirportSearchPageViewModel(IViewModelParameters viewModelParameters) : BaseViewModel(viewModelParameters)
    {
        private List<AirportListItem> allAirports = new();

        #region [ Properties ]

        [ObservableProperty]
        private AirportFieldType _fieldType;

        [ObservableProperty]
        private ObservableCollection<FlightRoute>? _airports;

        [ObservableProperty]
        private ObservableCollection<AirportListItem>? _filteredAirports;

        [ObservableProperty]
        private string? _searchText;

        [ObservableProperty]
        private string _pageTitle = string.Empty;

        [ObservableProperty]
        private string _searchPlaceholder = string.Empty;

        #endregion

        #region [ Methods ]

        partial void OnFieldTypeChanged(AirportFieldType value)
        {
            PageTitle = value == AirportFieldType.Source ? AppResource.SelectDepartureAirport : AppResource.SelectArrivalAirport;
            SearchPlaceholder = value == AirportFieldType.Source ? AppResource.SearchDepartureAirport : AppResource.SearchDestinationAirport;
        }

        partial void OnAirportsChanged(ObservableCollection<FlightRoute>? value)
        {
            allAirports = (value ?? new ObservableCollection<FlightRoute>())
                .Select(BuildListItem)
                .ToList();

            FilterAirports(SearchText);
        }

        partial void OnSearchTextChanged(string? value)
        {
            FilterAirports(value);
        }

        private AirportListItem BuildListItem(FlightRoute route)
        {
            // OriginRoute/DestinationRoute getters parse Sector and populate
            // OriginAiportName/DestinationAiportName as a side effect.
            string? code;
            string? name;
            if (FieldType == AirportFieldType.Source)
            {
                _ = route.OriginRoute;
                code = route.Origin;
                name = route.OriginAiportName;
            }
            else
            {
                _ = route.DestinationRoute;
                code = route.Destination;
                name = route.DestinationAiportName;
            }

            var airportInfo = code.GetAirportByIata();

            return new AirportListItem
            {
                Name = name,
                Code = code,
                City = airportInfo?.City,
                Country = airportInfo?.Country,
                Route = route
            };
        }

        private void FilterAirports(string? searchKey)
        {
            var results = string.IsNullOrWhiteSpace(searchKey)
                ? allAirports
                : allAirports.Where(x =>
                    x.Name.ContainsIgnoreCase(searchKey) ||
                    x.Code.ContainsIgnoreCase(searchKey) ||
                    x.City.ContainsIgnoreCase(searchKey) ||
                    x.Country.ContainsIgnoreCase(searchKey));

            FilteredAirports = new ObservableCollection<AirportListItem>(results);
        }

        #endregion

        #region [ Commands ]

        [RelayCommand]
        private async Task SelectAirport(AirportListItem? item)
        {
            if (item is null)
                return;

            await ShellNavigationService.NavigateBack(parameters: new Dictionary<string, object>
            {
                {
                    NavigationParamConstants.AirportSelectionResult,
                    new AirportSelectionResult { FieldType = FieldType, SelectedAirport = item.Route }
                }
            });
        }

        #endregion
    }
}
