using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IToolTransactionRepository : IGenericRepository<ToolTransaction>
    {
        Task<ToolTransaction?> GetByBorrowRequestIdAsync(int borrowRequestId);
        Task<IEnumerable<ToolTransaction>> GetActiveTransactionsAsync();
        Task<IEnumerable<ToolTransaction>> GetOverdueTransactionsAsync();
        Task<IEnumerable<ToolTransaction>> GetTransactionsByBorrowerAsync(int borrowerId);
        Task<IEnumerable<ToolTransaction>> GetTransactionsByOwnerAsync(int ownerId);
        Task<decimal> GetTotalFinesCollectedAsync();
    }
}
