using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.BLL.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<Payment?> GetPaymentByRequestIdAsync(int borrowRequestId);
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId);
        Task<Payment> ProcessPaymentAsync(Payment payment, int borrowerId);
        Task<decimal> GetTotalPaymentsByOwnerAsync(int ownerId);
    }
}
