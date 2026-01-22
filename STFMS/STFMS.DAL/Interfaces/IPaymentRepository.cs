using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetPaymentByBookingIdAsync(int bookingId);
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(PaymentMethod paymentMethod);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId);
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetRevenueByMethodAsync(PaymentMethod paymentMethod);
        Task<IEnumerable<Payment>> GetFailedPaymentsAsync();
        Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
    }
}
