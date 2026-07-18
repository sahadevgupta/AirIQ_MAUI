using AirIQ.Configurations.CustomExceptions;
using AirIQ.Constants;
using AirIQ.Models.Request;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

using Refit;

namespace AirIQ.Services
{
    public class OperationsService(IApiServiceBaseParams apiServiceBaseParams) : ApiServiceBase(apiServiceBaseParams), IOperationsService
    {
        public async Task<IEnumerable<SalesRecordDto>> GetSalesRecordsAsync(int agentId, int page, int pageSize)
        {
            IEnumerable<SalesRecordDto> salesRecordDtos = Enumerable.Empty<SalesRecordDto>();
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();
                var apiResponse = await BackendService.GetSalesRecordsAsync(agentId, headers, page, pageSize).ConfigureAwait(false);
                salesRecordDtos = apiResponse.Data ?? Enumerable.Empty<SalesRecordDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return salesRecordDtos;
        }

        public async Task<IEnumerable<RefundRecordDto>> GetRefundRecordsAsync(int agentId, int page, int pageSize)
        {
            IEnumerable<RefundRecordDto> refundRecordDtos = Enumerable.Empty<RefundRecordDto>();
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();
                var apiResponse = await BackendService.GetRefundRecordsAsync(agentId, headers, page, pageSize).ConfigureAwait(false);
                refundRecordDtos = apiResponse.Data ?? Enumerable.Empty<RefundRecordDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return refundRecordDtos;
        }

        public async Task<IEnumerable<AccountLedgerRecordDto>> GetAccountLedgerRecordsAsync(int agentId, int page, int pageSize)
        {
            IEnumerable<AccountLedgerRecordDto> accountLedgerRecordDtos = Enumerable.Empty<AccountLedgerRecordDto>();
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();
                var apiResponse = await BackendService.GetAccountLedgerRecordsAsync(agentId, headers, page, pageSize).ConfigureAwait(false);
                accountLedgerRecordDtos = apiResponse.Data ?? Enumerable.Empty<AccountLedgerRecordDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return accountLedgerRecordDtos;
        }

        public async Task<IEnumerable<TempCreditRecordDto>> GetTeempCreditRecordsAsync(int agentId, int page, int pageSize)
        {
            IEnumerable<TempCreditRecordDto> tempCreditRecordDtos = Enumerable.Empty<TempCreditRecordDto>();
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();
                var apiResponse = await BackendService.GetTempCreditRecordsAsync(agentId, headers, page, pageSize).ConfigureAwait(false);
                tempCreditRecordDtos = apiResponse.Data ?? Enumerable.Empty<TempCreditRecordDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return tempCreditRecordDtos;
        }

        public async Task<IEnumerable<GroupQueryResponseDto>> GetGroupQueryRecordsAsync(int agentId, int page, int pageSize)
        {
            IEnumerable<GroupQueryResponseDto> groupQueryRecordDtos = Enumerable.Empty<GroupQueryResponseDto>();
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();
                var apiResponse = await BackendService.GetGroupQueryRecordsAsync(agentId, headers, page, pageSize).ConfigureAwait(false);
                groupQueryRecordDtos = apiResponse.Data ?? Enumerable.Empty<GroupQueryResponseDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return groupQueryRecordDtos;
        }

        public async Task<IEnumerable<PaxCalendarFlightDto>> GetPaxCalendarFlightAsync(int agentId, string date)
        {
            IEnumerable<PaxCalendarFlightDto> paxCalendarFlightDto = Enumerable.Empty<PaxCalendarFlightDto>();
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();
                var apiResponse = await BackendService.GetPaxCalendarFlightAsync(agentId, headers, date).ConfigureAwait(false);
                paxCalendarFlightDto = apiResponse.Data ?? Enumerable.Empty<PaxCalendarFlightDto>();
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return paxCalendarFlightDto;
        }

        public async Task<bool> UploadRequestAsync(UploadRequest uploadRequest)
        {
            try
            {
                await Connectivity.CheckConnected();
                var headers = await GetHeader();

                using var stream = File.OpenRead(uploadRequest.FilePath ?? string.Empty);

                var streamPart = new StreamPart(
                    stream,
                    Path.GetFileName(uploadRequest.FilePath ?? string.Empty),
                    "image/jpeg"); // or image/png, application/pdf, etc.

                var apiResponse = await BackendService.SubmitUploadRequestAsync(
                                                       headers: headers,
                                                       accountName: AppConfiguration.CurrentUser?.AgencyName ?? string.Empty,
                                                       phoneNo: AppConfiguration.CurrentUser?.MobileNumber ?? string.Empty,
                                                       reference: uploadRequest.RefNumber ?? string.Empty,
                                                       mode: uploadRequest.PaymentMode ?? string.Empty,
                                                       amount: uploadRequest.Amount,
                                                       message: uploadRequest.Message ?? string.Empty,
                                                       file: streamPart)
                                                      .ConfigureAwait(false);

                return true;
            }
            catch (NotConnectedException notConntectedException)
            {
                HandleException(notConntectedException);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            return false;
        }
    }
}