using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IBorrowRequestRepository : IGenericRepository<BorrowRequest>
    {
        Task<IEnumerable<BorrowRequest>> GetRequestsByBorrowerAsync(int borrowerId);
        Task<IEnumerable<BorrowRequest>> GetRequestsByToolAsync(int toolId);
        Task<IEnumerable<BorrowRequest>> GetRequestsByOwnerAsync(int ownerId);
        Task<IEnumerable<BorrowRequest>> GetRequestsByStatusAsync(byte status);
        Task<IEnumerable<BorrowRequest>> GetPendingRequestsForOwnerAsync(int ownerId);
        Task<bool> HasOverlappingRequestsAsync(int toolId, DateTime startDate, DateTime endDate, int? excludeRequestId = null);
        Task<int> GetTotalRequestsCountAsync();
    }
}
