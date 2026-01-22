using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByBorrowRequestIdAsync(int borrowRequestId);
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId);
        Task<Payment?> GetByTransactionReferenceAsync(string transactionReference);
        Task<decimal> GetTotalPaymentsByOwnerAsync(int ownerId);
        Task<decimal> GetTotalPaymentsInPeriodAsync(DateTime startDate, DateTime endDate);
    }
}
