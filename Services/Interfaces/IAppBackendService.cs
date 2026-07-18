using AirIQ.Constants;
using AirIQ.Enums;
using AirIQ.Models.Request;
using AirIQ.Models.Response;

using Refit;

namespace AirIQ.Services.Interfaces
{
    [Headers("Content-Type: application/json")]
    public interface IAppBackendService
    {
        #region [ GET ]

        [Get(UrlConstants.Sectors)]
        Task<ServiceResponse<IEnumerable<FlightRouteDto>>> GetAvailableSectors([HeaderCollection] IDictionary<string, string> headers);

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

        [Get($"{UrlConstants.SalesRecords}/{{agentId}}")]
        Task<RecordServiceResponse<IEnumerable<SalesRecordDto>>> GetSalesRecordsAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize);

        [Get($"{UrlConstants.AccountLedgerRecords}/{{agentId}}")]
        Task<RecordServiceResponse<IEnumerable<AccountLedgerRecordDto>>> GetAccountLedgerRecordsAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize);

        [Get($"{UrlConstants.RefundRecords}/{{agentId}}")]
        Task<RecordServiceResponse<IEnumerable<RefundRecordDto>>> GetRefundRecordsAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize);

        [Get($"{UrlConstants.BankDetailsRecords}/{{agentId}}")]
        Task<IEnumerable<BankDetailsResponseDto>> GetBankDetailsRecordsAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize);

        [Get($"{UrlConstants.TemporaryCreditRecords}/{{agentId}}")]
        Task<RecordServiceResponse<IEnumerable<TempCreditRecordDto>>> GetTempCreditRecordsAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize);

        [Get($"{UrlConstants.GroupQueryRecords}/{{agentId}}")]
        Task<RecordServiceResponse<IEnumerable<GroupQueryResponseDto>>> GetGroupQueryRecordsAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("page")] int page,
            [AliasAs("pageSize")] int pageSize);

        [Get($"{UrlConstants.PaxCalendarFlight}/{{agentId}}")]
        Task<PaxCalendarResponseDto> GetPaxCalendarFlightAsync(int agentId,
            [HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("date")] string date);

        #endregion


        #region [ POST ]

        [Post(UrlConstants.Availability)]
        Task<ServiceResponse<IEnumerable<string>>> DatesAvailability([Body(BodySerializationMethod.Serialized)] BookingDatesAvailabilityRequest request,
            [HeaderCollection] IDictionary<string, string> headers);

        [Post(UrlConstants.Login)]
        Task<LoginDto> Login([Body(BodySerializationMethod.Serialized)] LoginRequest request,
            [HeaderCollection] IDictionary<string, string> headers);

        [Post(UrlConstants.LoginWithoutApiKey)]
        Task<LoginDto> LoginAsync([Body(BodySerializationMethod.Serialized)] LoginRequest request);

        [Post(UrlConstants.Search)]
        Task<ServiceResponse<IEnumerable<FlightSearchResultDto>>> SearchFlights([Body(BodySerializationMethod.Serialized)] FlightSearchRequest request,
            [HeaderCollection] IDictionary<string, string> headers);

        [Post(UrlConstants.Book)]
        Task<ServiceResponse<object>> TicketBooking([Body(BodySerializationMethod.Serialized)] TicketBookingRequest request,
            [HeaderCollection] IDictionary<string, string> headers);

        [Multipart]
        [Post(UrlConstants.UploadRequest)]
        Task<ApiResponse<string>> SubmitUploadRequestAsync([HeaderCollection] IDictionary<string, string> headers,
            [AliasAs("AccountName")] string accountName,
            [AliasAs("PhoneNo")] string phoneNo,
            [AliasAs("Reference")] string reference,
            [AliasAs("Mode")] string mode,
            [AliasAs("Amount")] decimal amount,
            [AliasAs("Message")] string message,
            [AliasAs("File")] StreamPart? file);

        #endregion
    }
}
