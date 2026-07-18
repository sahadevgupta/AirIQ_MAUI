using AirIQ.Models.Request;
using AirIQ.Models.Response;

namespace AirIQ.Services.Interfaces
{
    public interface IOperationsService
    {
        Task<IEnumerable<SalesRecordDto>> GetSalesRecordsAsync(int agentId, int page, int pageSize);
        Task<IEnumerable<RefundRecordDto>> GetRefundRecordsAsync(int agentId, int page, int pageSize);
        Task<IEnumerable<AccountLedgerRecordDto>> GetAccountLedgerRecordsAsync(int agentId, int page, int pageSize);
        Task<IEnumerable<TempCreditRecordDto>> GetTeempCreditRecordsAsync(int agentId, int page, int pageSize);
        Task<IEnumerable<GroupQueryResponseDto>> GetGroupQueryRecordsAsync(int agentId, int page, int pageSize);
        Task<bool> UploadRequestAsync(UploadRequest uploadRequest);
        Task<IEnumerable<PaxCalendarFlightDto>> GetPaxCalendarFlightAsync(int agentId, string date);
    }
}