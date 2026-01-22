using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IPaymentService
    {
        // curd signatures
        Task<Payment?> GetPaymentByIdAsync(int paymentId);
        Task<IEnumerable<Payment>> GetAllPaymentsAsync();
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int paymentId);

        // payment lookup signatures
        Task<Payment?> GetPaymentByBookingIdAsync(int bookingId);
        Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);

        // payment filtering signatures
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(PaymentMethod paymentMethod);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Payment>> GetFailedPaymentsAsync();

        // payment processing signatures
        Task<Payment> ProcessPaymentAsync(int bookingId, decimal amount, PaymentMethod paymentMethod);
        Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
        Task MarkPaymentAsCompletedAsync(int paymentId, string transactionId);
        Task MarkPaymentAsFailedAsync(int paymentId);
        Task RefundPaymentAsync(int paymentId);

        // revenue and statistics signatures
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetRevenueByMethodAsync(PaymentMethod paymentMethod);
        Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
