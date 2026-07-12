using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces
{
    public interface IOperationsService
    {
        Task<IEnumerable<SalesRecordDto>> GetSalesRecordsAsync(int agentId, int page, int pageSize);
    }
}