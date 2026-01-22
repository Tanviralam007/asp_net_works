using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<Payment?> GetByBorrowRequestIdAsync(int borrowRequestId)
        {
            return await _dbSet
                .Include(p => p.BorrowRequest)
                .FirstOrDefaultAsync(p => p.BorrowRequestId == borrowRequestId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.BorrowRequest.BorrowerId == userId)
                .Include(p => p.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment?> GetByTransactionReferenceAsync(string transactionReference)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.TransactionReference == transactionReference);
        }

        public async Task<decimal> GetTotalPaymentsByOwnerAsync(int ownerId)
        {
            return await _dbSet
                .Where(p => p.BorrowRequest.Tool.OwnerId == ownerId)
                .SumAsync(p => p.Amount);
        }

        public async Task<decimal> GetTotalPaymentsInPeriodAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .SumAsync(p => p.Amount);
        }
    }
}
