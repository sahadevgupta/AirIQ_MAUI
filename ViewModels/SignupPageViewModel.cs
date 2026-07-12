using System.Collections.ObjectModel;

using AirIQ.Configurations.Mapper;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views.ContentViews;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AirIQ.ViewModels;

public partial class SignupPageViewModel(IViewModelParameters viewModelParameters,
    ILookupService lookupService) : BaseViewModel(viewModelParameters)
{
    #region [ Properties ]

    [ObservableProperty]
    private ObservableCollection<StepBarModel> _steps = new();

    [ObservableProperty]
    private ObservableCollection<Country> _countries = new();

    [ObservableProperty]
    private ObservableCollection<State> _states = new();

    [ObservableProperty]
    private ObservableCollection<MainCity> _cities = new();

    [ObservableProperty]
    private ObservableCollection<District> _districts = new();

    [ObservableProperty]
    private ObservableCollection<LookupItem> _primaryBusinessTypes = new();

    [ObservableProperty]
    private ObservableCollection<LookupItem> _secondaryBusinessTypes = new();

    [ObservableProperty]
    private Country? _selectedCountry;

    [ObservableProperty]
    private State? _selectedState;

    [ObservableProperty]
    private City? _selectedCity;

    [ObservableProperty]
    private District? _selectedDistrict;

    [ObservableProperty]
    private LookupItem? _selectedPrimaryBusinessType;

    [ObservableProperty]
    private LookupItem? _selectedSecondaryBusinessType;

    [ObservableProperty]
    private int _stepListCount;

    private IEnumerable<StateDto> tempStates = Enumerable.Empty<StateDto>();
    private IEnumerable<CityDto> tempCities = Enumerable.Empty<CityDto>();
    private IEnumerable<MainCityDto> tempMainCities = Enumerable.Empty<MainCityDto>();
    private IEnumerable<DistrictDto> tempDistricts = Enumerable.Empty<DistrictDto>();
    private IEnumerable<LookupItemDto> tempLookupItems = Enumerable.Empty<LookupItemDto>();

    #endregion

    #region [ Methods & Service Calls ]

    partial void OnSelectedCountryChanged(Country? value)
    {
        var result = tempStates.Where(s => s.CountryId == value?.Id && s.Status.GetValueOrDefault());
        States = new ObservableCollection<State>(BackendToAppModelMapper.GetStates(result));
    }

    partial void OnSelectedStateChanged(State? value)
    {
        var result = tempDistricts.Where(d => d.State == value?.Name && d.Status.GetValueOrDefault());
        Districts = new ObservableCollection<District>(BackendToAppModelMapper.GetDistricts(result));
    }

    partial void OnSelectedDistrictChanged(District? value)
    {
        var result = tempMainCities.Where(c => c.State == value?.State && c.DistrictId == value.Id && c.Status.GetValueOrDefault());
        Cities = new ObservableCollection<MainCity>(BackendToAppModelMapper.GetMainCities(result));
    }

    partial void OnSelectedPrimaryBusinessTypeChanged(LookupItem? value)
    {
        var result = tempLookupItems
                        .Where(x => !string.Equals(x.Name, value!.Name, StringComparison.OrdinalIgnoreCase));

        SecondaryBusinessTypes = new ObservableCollection<LookupItem>(BackendToAppModelMapper.GetLookupItems(result));

    }

    private static ContentView CreateStepContent(int index)
    {
        return index switch
        {
            0 => new PersonalInformationView(),
            1 => new ContactInformationView(),
            2 => new BusinessInformationView(),
            _ => new ContentView()
        };
    }

    private void EnsureStepContentLoaded(int index)
    {
        if (index < 0 || index >= Steps.Count)
        {
            return;
        }

        if (Steps[index].MainContent is null)
        {
            Steps[index].MainContent = CreateStepContent(index);
        }
    }

    private void InitializeData()
    {
        _ = FetchListingDataAsync();

        Steps = new ObservableCollection<StepBarModel>()
        {
            new StepBarModel()
            {
                StepName="Personal",
                Status=StepBarStatus.InProgress,
                IsNotLast=true,
                IsFirst=true,
                MainContent=new PersonalInformationView(),
                IsCurrentContent=true,
            },
            new StepBarModel()
            {
                StepName="Contact",
                Status=StepBarStatus.Pending,
                IsNotLast=true,
                IsFirst=false,
                IsCurrentContent=false
            },
            new StepBarModel()
            {
                StepName="Business",
                Status=StepBarStatus.Pending,
                IsNotLast=false,
                IsFirst=false,
                IsCurrentContent=false
            }
        };
        StepListCount = Steps.Count;

        AddContentForSelectedStep();
    }

    private async Task FetchListingDataAsync()
    {
        try
        {
            var countriesTask = lookupService.GetCountriesAsync();
            var statesTask = lookupService.GetStatesAsync();
            var citiesTask = lookupService.GetCitiesAsync();
            var mainCitiesTask = lookupService.GetMainCitiesAsync();
            var districtsTask = lookupService.GetDistrictsAsync();
            var accountManagerTask = lookupService.GetAccountManagersAsync(AccountManagerType.BusinessType);

            await Task.WhenAll(
                countriesTask,
                statesTask,
                citiesTask,
                mainCitiesTask,
                districtsTask,
                accountManagerTask);

            var countriesTaskResponse = await countriesTask;
            tempStates = await statesTask;
            tempCities = await citiesTask;
            tempMainCities = await mainCitiesTask;
            tempDistricts = await districtsTask;
            tempLookupItems = await accountManagerTask;
            var lookupItemsDtos = tempLookupItems.ToList();

            var lookupItems = BackendToAppModelMapper.GetLookupItems(lookupItemsDtos
                                                     .Where(x => !string.Equals(x.Name, "None", StringComparison.OrdinalIgnoreCase)));

            Countries = new ObservableCollection<Country>(BackendToAppModelMapper.GetCountries(countriesTaskResponse));
            PrimaryBusinessTypes = new ObservableCollection<LookupItem>(lookupItems);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    #endregion

    #region [ StepBar Module ]

    public void AddContentForSelectedStep()
    {
        StepBarModel? currentStep = Steps.FirstOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
        if (index >= 0)
        {
            EnsureStepContentLoaded(index);

            ContentView? content = Steps[index].MainContent;
            bool isNotLast = Steps[index].IsNotLast;
            if (content != null)
            {
                // if (SubGrid.Children.Count >= 1)
                // {
                //     for (int i = 0; i < SubGrid.Children.Count; i++)
                //     {
                //         SubGrid.Children.RemoveAt(i);
                //     }
                // }

                // SubGrid.Children.Add(content, 0, 0);
            }
            //IsNotLastPage = isNotLast;
        }
    }

    public void NavigateToBackStep()
    {
        StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
        if (index > 0)
        {
            StepBarModel step = Steps.ElementAt(index);
            step.Status = StepBarStatus.Pending;
            step.IsCurrentContent = false;
            Steps[index] = step;
            if ((index - 1) > 0)
            {
                StepBarModel stepnext = Steps.ElementAt(index - 1);
                if (stepnext.Status == StepBarStatus.Completed)
                {
                    stepnext.Status = StepBarStatus.InProgress;
                }
                EnsureStepContentLoaded(index - 1);
                stepnext.IsCurrentContent = true;
                Steps[index - 1] = stepnext;
            }
            else
            {
                StepBarModel stepnext = Steps.ElementAt(0);

                stepnext.Status = StepBarStatus.InProgress;

                EnsureStepContentLoaded(0);
                stepnext.IsCurrentContent = true;
                Steps[index - 1] = stepnext;
            }
            // int indexforstepper = MainGrid.Children.IndexOf(MainGrid.Children.LastOrDefault(x => x.GetType() == typeof(StepBarComponentView)));
            // MainGrid.Children.RemoveAt(indexforstepper);
            // MainGrid.Children.Add(stepbar, 0, 0);
        }
        AddContentForSelectedStep();

    }

    public void NavigateToNextStep()
    {
        //if (ValidateFields())
        {


            // if (isCreditCard && string.IsNullOrEmpty(CardNumber))
            //     CardDesc = $"{AppResource.ResourceManager.GetString("NotificationViewExpires", AppResource.Culture)} {CardExpirationDate}";

            // ShippingAddressDesc = shippingAddress.FormatAddress;
            // BillingingAddressDesc = billingAddress?.FormatAddress;

            StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
            if (currentStep is null)
            {
                return;
            }

            int index = Steps.IndexOf(currentStep);
            if (index >= 0 && index < Steps.Count)
            {
                StepBarModel step = Steps.ElementAt(index);
                step.Status = StepBarStatus.Completed;
                step.IsCurrentContent = false;
                Steps[index] = step;
                if ((index + 1) < Steps.Count)
                {
                    StepBarModel stepnext = Steps.ElementAt(index + 1);
                    if (stepnext.Status == StepBarStatus.Pending)
                    {
                        stepnext.Status = StepBarStatus.InProgress;
                    }
                    EnsureStepContentLoaded(index + 1);
                    stepnext.IsCurrentContent = true;
                    Steps[index + 1] = stepnext;
                }
                // int indexforstepper = MainGrid.Children.IndexOf(MainGrid.Children.FirstOrDefault(x => x.GetType() == typeof(StepBarComponentView)));
                // MainGrid.Children.RemoveAt(indexforstepper);

                // MainGrid.Children.Add(stepbar, 0, 0);
            }
            AddContentForSelectedStep();
        }

    }

    #endregion

    #region [ Commands ]

    [RelayCommand]
    private async Task NextStep()
    {
        StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
        if (index >= 0 && index < Steps.Count)
        {
            StepBarModel step = Steps.ElementAt(index);
            step.Status = StepBarStatus.Completed;
            step.IsCurrentContent = false;

            if ((index + 1) < Steps.Count)
            {
                EnsureStepContentLoaded(index + 1);
                StepBarModel stepnext = Steps.ElementAt(index + 1);
                if (stepnext.Status == StepBarStatus.Pending)
                {
                    stepnext.IsCurrentContent = true;
                    stepnext.Status = StepBarStatus.InProgress;
                }
            }
        }
    }

    [RelayCommand]
    private async Task BackStep()
    {
        StepBarModel? currentStep = Steps.LastOrDefault(x => x.IsCurrentContent);
        if (currentStep is null)
        {
            return;
        }

        int index = Steps.IndexOf(currentStep);
        if (index > 0)
        {
            StepBarModel step = Steps.ElementAt(index);
            step.Status = StepBarStatus.Pending;
            step.IsCurrentContent = false;
            if ((index - 1) > 0)
            {
                StepBarModel stepnext = Steps.ElementAt(index - 1);
                if (stepnext.Status == StepBarStatus.Completed)
                {
                    stepnext.Status = StepBarStatus.InProgress;
                }
                EnsureStepContentLoaded(index - 1);
                stepnext.IsCurrentContent = true;
            }
            else
            {
                StepBarModel stepnext = Steps.ElementAt(0);
                stepnext.Status = StepBarStatus.InProgress;
                EnsureStepContentLoaded(0);
                stepnext.IsCurrentContent = true;
            }
        }
    }

    #endregion

    #region [ Override Methods ]

    public override Task LoadDataWhenNavigatedTo()
    {
        InitializeData();
        return base.LoadDataWhenNavigatedTo();
    }

    #endregion

}
