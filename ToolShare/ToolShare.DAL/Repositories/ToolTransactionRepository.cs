using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class ToolTransactionRepository : GenericRepository<ToolTransaction>, IToolTransactionRepository
    {
        public ToolTransactionRepository(AppDbContext context) : base(context) { }

        public override async Task<ToolTransaction?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Borrower)
                .Include(tt => tt.Payment)
                .FirstOrDefaultAsync(tt => tt.Id == id);
        }

        public async Task<ToolTransaction?> GetByBorrowRequestIdAsync(int borrowRequestId)
        {
            return await _dbSet
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .Include (tt => tt.BorrowRequest)
                .ThenInclude(br => br.Borrower)
                .Include(tt => tt.Payment)
                .FirstOrDefaultAsync(tt => tt.BorrowRequestId == borrowRequestId);
        }

        public async Task<IEnumerable<ToolTransaction>> GetActiveTransactionsAsync()
        {
            return await _dbSet
                .Where(tt => (byte)tt.Status == 1 && tt.ReturnDate == null)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Borrower)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToolTransaction>> GetOverdueTransactionsAsync()
        {
            var today = DateTime.Now;
            return await _dbSet
                .Where(tt => (byte)tt.Status == 1 &&
                            tt.ReturnDate == null &&
                            tt.ExpectedReturnDate < today)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Borrower)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToolTransaction>> GetTransactionsByBorrowerAsync(int borrowerId)
        {
            return await _dbSet
                .Where(tt => tt.BorrowRequest.BorrowerId == borrowerId)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Borrower)
                .OrderByDescending(tt => tt.HandoverDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToolTransaction>> GetTransactionsByOwnerAsync(int ownerId)
        {
            return await _dbSet
                .Where(tt => tt.BorrowRequest.Tool.OwnerId == ownerId)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Tool)
                .Include(tt => tt.BorrowRequest)
                .ThenInclude(br => br.Borrower)
                .OrderByDescending(tt => tt.HandoverDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalFinesCollectedAsync()
        {
            return await _dbSet
                .Where(tt => tt.FineAmount > 0)
                .SumAsync(tt => tt.FineAmount);
        }
    }
}
