using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IToolTransactionService
    {
        Task<ToolTransaction?> GetTransactionByIdAsync(int id);
        Task<ToolTransaction?> GetTransactionByRequestIdAsync(int borrowRequestId);
        Task<IEnumerable<ToolTransaction>> GetActiveTransactionsAsync();
        Task<IEnumerable<ToolTransaction>> GetOverdueTransactionsAsync();
        Task<IEnumerable<ToolTransaction>> GetTransactionsByBorrowerAsync(int borrowerId);
        Task<IEnumerable<ToolTransaction>> GetTransactionsByOwnerAsync(int ownerId);
        Task<ToolTransaction> ConfirmHandoverAsync(int requestId, int ownerId);
        Task<ToolTransaction> ProcessReturnAsync(int transactionId, int ownerId);
        Task<decimal> CalculateFineAsync(DateTime expectedReturnDate, DateTime actualReturnDate, decimal dailyRate);
    }
}
