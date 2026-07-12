using AirIQ.Configurations.CustomExceptions;
using AirIQ.Models.Response;
using AirIQ.Services.Interfaces;

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
    }
}