using Microsoft.EntityFrameworkCore;
using ToolShare.DAL.Data;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.DAL.Repositories
{
    public class BorrowRequestRepository : GenericRepository<BorrowRequest>, IBorrowRequestRepository
    {
        public BorrowRequestRepository(AppDbContext context) : base(context) { }

        // Override to include navigation properties
        public override async Task<IEnumerable<BorrowRequest>> GetAllAsync()
        {
            return await _dbSet
                .Include(br => br.Tool)
                .Include(br => br.Borrower)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        // Override to include navigation properties
        public override async Task<BorrowRequest?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(br => br.Tool)
                .Include(br => br.Borrower)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByBorrowerAsync(int borrowerId)
        {
            return await _dbSet
                .Where(br => br.BorrowerId == borrowerId)
                .Include(br => br.Tool)
                .ThenInclude(t => t.Category)
                .Include(br => br.Borrower)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByToolAsync(int toolId)
        {
            return await _dbSet
                .Where(br => br.ToolId == toolId)
                .Include(br => br.Tool)
                .Include(br => br.Borrower)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByOwnerAsync(int ownerId)
        {
            return await _dbSet
                .Where(br => br.Tool.OwnerId == ownerId)
                .Include(br => br.Tool)
                .Include(br => br.Borrower)
                .OrderByDescending(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByStatusAsync(byte status)
        {
            return await _dbSet
                .Where(br => (byte)br.Status == status)
                .Include(br => br.Tool)
                .Include(br => br.Borrower)
                .ToListAsync();
        }

        public async Task<IEnumerable<BorrowRequest>> GetPendingRequestsForOwnerAsync(int ownerId)
        {
            return await _dbSet
                .Where(br => br.Tool.OwnerId == ownerId && br.Status == RequestStatus.Pending) // Pending
                .Include(br => br.Tool)
                .Include(br => br.Borrower)
                .OrderBy(br => br.RequestDate)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingRequestsAsync(int toolId, DateTime startDate,
            DateTime endDate, int? excludeRequestId = null)
        {
            var query = _dbSet.Where(br =>
                br.ToolId == toolId &&
                (br.Status == RequestStatus.Pending || br.Status == RequestStatus.Approved) && // Pending or Approved
                ((br.StartDate <= endDate && br.EndDate >= startDate)));

            if (excludeRequestId.HasValue)
                query = query.Where(br => br.Id != excludeRequestId.Value);

            return await query.AnyAsync();
        }
        public async Task<int> GetTotalRequestsCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
