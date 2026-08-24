using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using Refit;

namespace AirIQ.Services.Interfaces
{
    public interface IAuthenticationApi
    {
        #region [ GET ]

        [Get(UrlConstants.Countries)]
        Task<IEnumerable<CountryDto>> GetCountries();

        [Get(UrlConstants.States)]
        Task<IEnumerable<StateDto>> GetStates();

        [Get(UrlConstants.Cities)]
        Task<IEnumerable<CityDto>> GetCities();

        [Get(UrlConstants.MainCities)]
        Task<IEnumerable<MainCityDto>> GetMainCities();

        [Get(UrlConstants.Districts)]
        Task<IEnumerable<DistrictDto>> GetDistricts();

        [Get(UrlConstants.Accountmanagers)]
        Task<IEnumerable<LookupItemDto>> GetAccountManagers([AliasAs("type")] AccountManagerType type);

        #endregion

        #region [ POST ]

        [Post(UrlConstants.LoginWithoutApiKey)]
        Task<LoginDto> LoginAsync([Body(BodySerializationMethod.Serialized)] LoginRequest request);

        [Post(UrlConstants.Signup)]
        Task<SignupDto> SignupAsync([Body(BodySerializationMethod.Serialized)] SignupRequest request);

        [Post(UrlConstants.ForgotPassword)]
        Task<ForgotPasswordDto> ForgotPasswordAsync([Body(BodySerializationMethod.Serialized)] ForgotPasswordRequest request);

        [Post(UrlConstants.VerifyOTP)]
        Task<ApiResponse<string>> VerifyOtpAsync([Body(BodySerializationMethod.Serialized)] VerifyOtpRequest request);

        [Post(UrlConstants.ResetPassword)]
        Task<ApiResponse<string>> ResetPasswordAsync([Body(BodySerializationMethod.Serialized)] ResetPasswordRequest request);

        [Post(UrlConstants.ChangePassword)]
        Task<ApiResponse<string>> ChangePasswordAsync([Body(BodySerializationMethod.Serialized)] ChangePasswordRequest request);

        #endregion
    }
}