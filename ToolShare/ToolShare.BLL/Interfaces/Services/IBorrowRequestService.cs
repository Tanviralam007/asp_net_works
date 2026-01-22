using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IBorrowRequestService
    {
        Task<IEnumerable<BorrowRequest>> GetAllRequestsAsync();
        Task<BorrowRequest?> GetRequestByIdAsync(int id);
        Task<IEnumerable<BorrowRequest>> GetRequestsByBorrowerAsync(int borrowerId);
        Task<IEnumerable<BorrowRequest>> GetRequestsByToolAsync(int toolId);
        Task<IEnumerable<BorrowRequest>> GetRequestsByOwnerAsync(int ownerId);
        Task<IEnumerable<BorrowRequest>> GetPendingRequestsForOwnerAsync(int ownerId);
        Task<BorrowRequest> CreateRequestAsync(BorrowRequest request, int borrowerId);
        Task<bool> CancelRequestAsync(int requestId, int borrowerId);
        Task<BorrowRequest> ApproveRequestAsync(int requestId, int ownerId);
        Task<BorrowRequest> RejectRequestAsync(int requestId, int ownerId);
        Task<decimal> CalculateRentalCostAsync(int toolId, DateTime startDate, DateTime endDate);
    }
}
