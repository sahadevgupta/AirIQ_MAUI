using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using AirIQ.Constants;
using AirIQ.Configurations.Mapper;
using AirIQ.Enums;
using AirIQ.Models;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Resources.Strings;
using AirIQ.Services.Interfaces;
using AirIQ.ViewModels.Common;
using AirIQ.Views.ContentViews;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AirIQ.Views;

namespace AirIQ.ViewModels;

public partial class SignupPageViewModel : BaseViewModel
{
    #region [ Properties ]

    private readonly IDialogService _dialogService;
    private readonly ILookupService _lookupService;
    private readonly IZoopVerificationService _zoopVerificationService;
    private readonly IAuthenticationService _authenticationService;

    [ObservableProperty]
    private ObservableCollection<StepBarModel> _steps = new();

    [ObservableProperty]
    private ObservableCollection<Country> _countries = new();

    [ObservableProperty]
    private ObservableCollection<State> _states = new();

    [ObservableProperty]
    private ObservableCollection<MainCity> _cities = new();

    [ObservableProperty]
    private ObservableCollection<City> _nearestAirport = new();

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
    private MainCity? _selectedCity;

    [ObservableProperty]
    private City? _selectedAirport;

    [ObservableProperty]
    private District? _selectedDistrict;

    [ObservableProperty]
    private LookupItem? _selectedPrimaryBusinessType;

    [ObservableProperty]
    private LookupItem? _selectedSecondaryBusinessType;

    [ObservableProperty]
    private int _stepListCount;

    [ObservableProperty]
    private string? _primaryMonthlyIncome;

    [ObservableProperty]
    private string? _secondaryMonthlyIncome;

    [ObservableProperty]
    private string? _referredBy;

    [ObservableProperty]
    private string? _companyName;

    [ObservableProperty]
    private string? _fullName;

    [ObservableProperty]
    private string? _phoneNumber;

    [ObservableProperty]
    private string? _emailAddress;

    [ObservableProperty]
    private SignupRequest _signup = new();

    [ObservableProperty]
    private bool _isCompanyNameErrorVisible;
    [ObservableProperty]
    private bool _isFullNameErrorVisible;
    [ObservableProperty]
    private bool _isPhoneNumberErrorVisible;
    [ObservableProperty]
    private bool _isEmailErrorVisible;
    [ObservableProperty]
    private bool _isWhatsAppNumberErrorVisible;
    [ObservableProperty]
    private bool _isReferredByErrorVisible;
    [ObservableProperty]
    private bool _isPasswordErrorVisible;
    [ObservableProperty]
    private bool _isConfirmPasswordErrorVisible;

    [ObservableProperty]
    private bool _isCountryErrorVisible;
    [ObservableProperty]
    private bool _isStateErrorVisible;
    [ObservableProperty]
    private bool _isDistrictErrorVisible;
    [ObservableProperty]
    private bool _isCityErrorVisible;
    [ObservableProperty]
    private bool _isNearestAirportErrorVisible;
    [ObservableProperty]
    private bool _isPrimaryBusinessTypeErrorVisible;
    [ObservableProperty]
    private bool _isPrimaryMonthlyIncomeErrorVisible;
    [ObservableProperty]
    private bool _isSecondaryBusinessTypeErrorVisible;
    [ObservableProperty]
    private bool _isSecondaryMonthlyIncomeErrorVisible;

    [ObservableProperty]
    private string? _whatsAppNumber;

    [ObservableProperty]
    private string? _password;

    [ObservableProperty]
    private string? _confirmPassword;

    [ObservableProperty]
    private bool _isPanValid;

    [ObservableProperty]
    private string? _panRegisterdName;

    [ObservableProperty]
    private bool _isGstValid;

    [ObservableProperty]
    private string? _gstRegisterdName;

    [ObservableProperty]
    private string? _salesPersonName;


    private IEnumerable<StateDto> tempStates = Enumerable.Empty<StateDto>();
    private IEnumerable<CityDto> tempCities = Enumerable.Empty<CityDto>();
    private IEnumerable<MainCityDto> tempMainCities = Enumerable.Empty<MainCityDto>();
    private IEnumerable<DistrictDto> tempDistricts = Enumerable.Empty<DistrictDto>();
    private IEnumerable<LookupItemDto> tempLookupItems = Enumerable.Empty<LookupItemDto>();
    private IEnumerable<LookupItemDto> sourceAccountManagerItems = Enumerable.Empty<LookupItemDto>();

    #endregion

    public SignupPageViewModel(IViewModelParameters viewModelParameters,
        ILookupService lookupService,
        IZoopVerificationService zoopVerificationService,
        IDialogService dialogService,
        IAuthenticationService authenticationService) : base(viewModelParameters)
    {
        _lookupService = lookupService;
        _zoopVerificationService = zoopVerificationService;
        _dialogService = dialogService;
        _authenticationService = authenticationService;
    }

    #region [ Methods & Service Calls ]

    partial void OnSelectedCountryChanged(Country? value)
    {
        IsCountryErrorVisible = value is null;
        SelectedState = null;
        SelectedDistrict = null;
        SelectedCity = null;
        SelectedAirport = null;
        var result = tempStates.Where(s => s.CountryId == value?.Id && s.Status.GetValueOrDefault());
        States = new ObservableCollection<State>(BackendToAppModelMapper.GetStates(result));
    }

    partial void OnSelectedStateChanged(State? value)
    {
        IsStateErrorVisible = value is null;
        SelectedDistrict = null;
        SelectedCity = null;
        SelectedAirport = null;
        var result = tempDistricts.Where(d => d.State == value?.Name && d.Status.GetValueOrDefault());
        Districts = new ObservableCollection<District>(BackendToAppModelMapper.GetDistricts(result));
    }

    partial void OnSelectedDistrictChanged(District? value)
    {
        IsDistrictErrorVisible = value is null;
        SelectedCity = null;
        SelectedAirport = null;
        var result = tempMainCities.Where(c => c.State == value?.State && c.DistrictId == value?.Id && c.Status.GetValueOrDefault());
        Cities = new ObservableCollection<MainCity>(BackendToAppModelMapper.GetMainCities(result));
    }

    partial void OnSelectedCityChanged(MainCity? value)
    {
        IsCityErrorVisible = value is null;
        SelectedAirport = null;
        var result = tempCities.Where(c => c.State == value?.State && c.Status.GetValueOrDefault());
        NearestAirport = new ObservableCollection<City>(BackendToAppModelMapper.GetCities(result));
    }

    partial void OnSelectedPrimaryBusinessTypeChanged(LookupItem? value)
    {
        IsPrimaryBusinessTypeErrorVisible = value is null;

        if (value is null)
        {
            SecondaryBusinessTypes = new ObservableCollection<LookupItem>();
            return;
        }

        var result = tempLookupItems
                        .Where(x => !string.Equals(x.Name, value.Name, StringComparison.OrdinalIgnoreCase));

        SecondaryBusinessTypes = new ObservableCollection<LookupItem>(BackendToAppModelMapper.GetLookupItems(result));

    }

    partial void OnSelectedAirportChanged(City? value)
    {
        IsNearestAirportErrorVisible = value is null;
    }

    partial void OnCompanyNameChanged(string? value)
    {
        Signup.CompanyName = value;
        IsCompanyNameErrorVisible = string.IsNullOrWhiteSpace(value);
    }

    partial void OnFullNameChanged(string? value)
    {
        IsFullNameErrorVisible = !IsValidFullName(value);
    }

    partial void OnPhoneNumberChanged(string? value)
    {
        Signup.Phone = value?.Trim();
        IsPhoneNumberErrorVisible = !IsValidIndianMobileNumber(Signup.Phone);
    }

    partial void OnEmailAddressChanged(string? value)
    {
        Signup.Email = value;
        IsEmailErrorVisible = string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, AppConstants.EmailRegex);
    }

    partial void OnWhatsAppNumberChanged(string? value)
    {
        IsWhatsAppNumberErrorVisible = string.IsNullOrWhiteSpace(value);
    }

    partial void OnReferredByChanged(string? value)
    {
        IsReferredByErrorVisible = string.IsNullOrWhiteSpace(value);
    }

    partial void OnPasswordChanged(string? value)
    {
        IsPasswordErrorVisible = string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, AppConstants.PasswordRegex);
        IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(ConfirmPassword)
                                     || !string.Equals(value, ConfirmPassword, StringComparison.Ordinal);
    }

    partial void OnConfirmPasswordChanged(string? value)
    {
        IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(value)
                                     || !string.Equals(Password, value, StringComparison.Ordinal);
    }

    partial void OnPrimaryMonthlyIncomeChanged(string? value)
    {
        IsPrimaryMonthlyIncomeErrorVisible = string.IsNullOrWhiteSpace(value);
    }

    partial void OnSelectedSecondaryBusinessTypeChanged(LookupItem? value)
    {
        IsSecondaryBusinessTypeErrorVisible = !string.IsNullOrWhiteSpace(SecondaryMonthlyIncome) && value is null;
    }

    partial void OnSecondaryMonthlyIncomeChanged(string? value)
    {
        IsSecondaryMonthlyIncomeErrorVisible = SelectedSecondaryBusinessType is not null
                                              && string.IsNullOrWhiteSpace(value);
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

    private async Task InitializeData()
    {
        using (LoadingService.Show())
        {
            sourceAccountManagerItems = await _lookupService.GetAccountManagersAsync(AccountManagerType.Source);
            Steps = new ObservableCollection<StepBarModel>()
            {
                new StepBarModel()
                {
                    StepName=AppResource.PersonalStepName,
                    Status=StepBarStatus.InProgress,
                    IsNotLast=true,
                    IsFirst=true,
                    MainContent=new PersonalInformationView(),
                    IsCurrentContent=true,
                },
                new StepBarModel()
                {
                    StepName=AppResource.ContactStepName,
                    Status=StepBarStatus.Pending,
                    IsNotLast=true,
                    IsFirst=false,
                    IsCurrentContent=false
                },
                new StepBarModel()
                {
                    StepName=AppResource.BusinessStepName,
                    Status=StepBarStatus.Pending,
                    IsNotLast=false,
                    IsFirst=false,
                    IsCurrentContent=false
                }
            };
            StepListCount = Steps.Count;

            AddContentForSelectedStep();
        }

        _ = FetchListingDataAsync();
    }

    private async Task FetchListingDataAsync()
    {
        try
        {
            var countriesTask = _lookupService.GetCountriesAsync();
            var statesTask = _lookupService.GetStatesAsync();
            var citiesTask = _lookupService.GetCitiesAsync();
            var mainCitiesTask = _lookupService.GetMainCitiesAsync();
            var districtsTask = _lookupService.GetDistrictsAsync();
            var accountManagerTask = _lookupService.GetAccountManagersAsync(AccountManagerType.BusinessType);

            await Task.WhenAll(
                countriesTask,
                statesTask,
                citiesTask,
                mainCitiesTask,
                districtsTask,
                accountManagerTask);

            tempStates = await statesTask;
            tempCities = await citiesTask;
            tempMainCities = await mainCitiesTask;
            tempDistricts = await districtsTask;
            tempLookupItems = await accountManagerTask;
            var lookupItemsDtos = tempLookupItems.ToList();

            var lookupItems = BackendToAppModelMapper.GetLookupItems(lookupItemsDtos
                                                     .Where(x => !string.Equals(x.Name, "None", StringComparison.OrdinalIgnoreCase)));

            Countries = new ObservableCollection<Country>(BackendToAppModelMapper.GetCountries(await countriesTask));

            PrimaryBusinessTypes = new ObservableCollection<LookupItem>(lookupItems);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }
    }

    private void CreateSignupRequest()
    {
        var (firstName, lastName) = SplitFullName(FullName);

        Signup.CompanyName = CompanyName;
        Signup.FName = firstName;
        Signup.LName = lastName;
        Signup.Phone = PhoneNumber?.Trim();
        Signup.Email = EmailAddress;
        Signup.Password = Password;
        Signup.Country = SelectedCountry?.Name;
        Signup.State = SelectedState?.Name;
        Signup.CountryId = SelectedCountry?.Id.ToString();
        Signup.DistrictId = SelectedDistrict?.Id.ToString();
        Signup.CityEntryMainId = SelectedCity?.CityEntryMainId.ToString();
        Signup.CityId = SelectedAirport?.Id.ToString();
        Signup.PrimaryBusinessId = SelectedPrimaryBusinessType?.Id.ToString();
        Signup.SecondaryBusinessId = SelectedSecondaryBusinessType is null ? null : SelectedSecondaryBusinessType?.Id.ToString();
        Signup.MonthlyValue = PrimaryMonthlyIncome;
        Signup.MonthlyValue2 = SecondaryMonthlyIncome;
    }

    private bool ValidateCurrentStep(int index)
    {
        return index switch
        {
            0 => ValidatePersonalInformationStep(),
            1 => ValidateContactInformationStep(),
            2 => ValidateBusinessInformationStep(),
            _ => true
        };
    }

    private bool ValidatePersonalInformationStep()
    {
        IsCompanyNameErrorVisible = string.IsNullOrWhiteSpace(CompanyName);
        IsFullNameErrorVisible = !IsValidFullName(FullName);
        IsPhoneNumberErrorVisible = !IsValidIndianMobileNumber(PhoneNumber);
        IsEmailErrorVisible = string.IsNullOrWhiteSpace(EmailAddress)
                           || !Regex.IsMatch(EmailAddress, AppConstants.EmailRegex);
        IsWhatsAppNumberErrorVisible = string.IsNullOrWhiteSpace(WhatsAppNumber);
        IsReferredByErrorVisible = string.IsNullOrWhiteSpace(ReferredBy);
        IsPasswordErrorVisible = string.IsNullOrWhiteSpace(Password)
                              || !Regex.IsMatch(Password, AppConstants.PasswordRegex);
        IsConfirmPasswordErrorVisible = string.IsNullOrWhiteSpace(ConfirmPassword)
                                     || !string.Equals(Password, ConfirmPassword, StringComparison.Ordinal);

        return !IsCompanyNameErrorVisible
            && !IsFullNameErrorVisible
            && !IsPhoneNumberErrorVisible
            && !IsEmailErrorVisible
            && !IsWhatsAppNumberErrorVisible
            && !IsReferredByErrorVisible
            && !IsPasswordErrorVisible
            && !IsConfirmPasswordErrorVisible;
    }

    private static bool IsValidIndianMobileNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        return Regex.IsMatch(phoneNumber.Trim(), @"^[6-9]\d{9}$");
    }

    private static bool IsValidFullName(string? fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return false;
        }

        return Regex.IsMatch(fullName.Trim(), @"^[^\s]+ [^\s]+$");
    }

    private static (string? FirstName, string? LastName) SplitFullName(string? fullName)
    {
        if (!IsValidFullName(fullName))
        {
            return (null, null);
        }

        var parts = fullName!.Trim().Split(' ', 2, StringSplitOptions.None);
        return (parts[0], parts[1]);
    }

    private bool ValidateContactInformationStep()
    {
        IsCountryErrorVisible = SelectedCountry is null;
        IsStateErrorVisible = SelectedState is null;
        IsDistrictErrorVisible = SelectedDistrict is null;
        IsCityErrorVisible = SelectedCity is null;
        IsNearestAirportErrorVisible = SelectedAirport is null;

        return !IsCountryErrorVisible
            && !IsStateErrorVisible
            && !IsDistrictErrorVisible
            && !IsCityErrorVisible
            && !IsNearestAirportErrorVisible;
    }

    private bool ValidateBusinessInformationStep()
    {
        IsPrimaryBusinessTypeErrorVisible = SelectedPrimaryBusinessType is null;
        IsPrimaryMonthlyIncomeErrorVisible = string.IsNullOrWhiteSpace(PrimaryMonthlyIncome);

        IsSecondaryBusinessTypeErrorVisible = !string.IsNullOrWhiteSpace(SecondaryMonthlyIncome)
                                           && SelectedSecondaryBusinessType is null;
        IsSecondaryMonthlyIncomeErrorVisible = SelectedSecondaryBusinessType is not null
                                            && string.IsNullOrWhiteSpace(SecondaryMonthlyIncome);

        return !IsPrimaryBusinessTypeErrorVisible
            && !IsPrimaryMonthlyIncomeErrorVisible
            && !IsSecondaryBusinessTypeErrorVisible
            && !IsSecondaryMonthlyIncomeErrorVisible;
    }

    private bool ValidateMandatoryFieldsForSubmit()
    {
        var isPersonalInfoValid = ValidatePersonalInformationStep();
        var isContactInfoValid = ValidateContactInformationStep();
        var isBusinessInfoValid = ValidateBusinessInformationStep();

        return isPersonalInfoValid && isContactInfoValid && isBusinessInfoValid;
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
        if (index < 0 || index >= Steps.Count)
        {
            return;
        }

        if (!ValidateCurrentStep(index))
        {
            return;
        }

        if (index == Steps.Count - 1)
        {
            await SubmitAsync();
            return;
        }

        NavigateToNextStep();
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

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsBusy)
            return;

        if (!ValidateMandatoryFieldsForSubmit())
        {
            await _dialogService.ShowStatusAlertAsync(AppResource.FillMandatoryFieldsCorrectly, false, 3000);
            return;
        }

        try
        {
            IsBusy = true;
            CreateSignupRequest();
            var result = sourceAccountManagerItems.FirstOrDefault(x => x.Name?.Equals(ReferredBy, StringComparison.OrdinalIgnoreCase) == true);

            using (LoadingService.Show())
            {
                var response = await _authenticationService.SignupAsync(Signup);
                if (!string.IsNullOrWhiteSpace(response) && response.Contains("Agency registration submitted successfully."))
                {
                    string message = AppResource.RegistrationSubmittedSuccessMessage;
                    await _dialogService.ShowAlertDialog(message, AlertType.Success);

                    await ShellNavigationService.Navigate<LoginPage>(true);
                }
                else
                {
                    await _dialogService.ShowAlertDialog(response, AlertType.Error);
                }
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ValidatePanAsync()
    {
        if (string.IsNullOrWhiteSpace(Signup.PanNo) || string.IsNullOrWhiteSpace(FullName))
            return;

        using (LoadingService.Show())
        {
            var response = await _zoopVerificationService.ValidatePanAsync(Signup.PanNo, FullName);
            if (response is not null)
            {
                if (response.PanType?.Equals("Person", StringComparison.OrdinalIgnoreCase) == true)
                {
                    IsPanValid = response.UserFullName?.Equals(FullName, StringComparison.OrdinalIgnoreCase) == true;

                    if (IsPanValid)
                    {
                        PanRegisterdName = response.UserFullName;
                    }
                    else
                    {
                        PanRegisterdName = string.Empty;

                        await _dialogService.ShowStatusAlertAsync(AppResource.OwnerNamePanNameMismatch, false, 3500);
                    }

                }
                else if (response.PanType?.Equals("Firm", StringComparison.OrdinalIgnoreCase) == true ||
                   response.PanType?.Equals("Comapny", StringComparison.OrdinalIgnoreCase) == true)
                {
                    IsPanValid = response.UserFullName?.Equals(CompanyName, StringComparison.OrdinalIgnoreCase) == true;
                    if (IsPanValid)
                    {
                        PanRegisterdName = response.UserFullName;
                    }
                    else
                    {
                        PanRegisterdName = string.Empty;

                        await _dialogService.ShowStatusAlertAsync(AppResource.CompanyNamePanNameMismatch, false, 3500);
                    }
                }
            }
        }
    }

    [RelayCommand]
    private async Task ValidateGstNoAsync()
    {
        if (string.IsNullOrWhiteSpace(Signup.GstNo))
            return;

        using (LoadingService.Show())
        {
            var response = await _zoopVerificationService.ValidateGstAsync(Signup.GstNo);
            IsGstValid = response is not null;
            GstRegisterdName = IsGstValid ? response?.TradeName : string.Empty;
        }
    }

    #endregion

    #region [ Override Methods ]

    public override async Task LoadDataWhenNavigatedTo()
    {
        await InitializeData();
    }

    #endregion
}


