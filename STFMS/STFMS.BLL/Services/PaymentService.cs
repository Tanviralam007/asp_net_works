using STFMS.BLL.Interfaces.Services;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;

        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
        }

        // curd
        public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
        {
            return await _paymentRepository.GetByIdAsync(paymentId);
        }

        public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
        {
            return await _paymentRepository.GetAllAsync();
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            // Business validation
            var booking = await _bookingRepository.GetByIdAsync(payment.BookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {payment.BookingId} not found.");
            }

            // Check if payment already exists for this booking
            var existingPayment = await _paymentRepository.GetPaymentByBookingIdAsync(payment.BookingId);
            if (existingPayment != null)
            {
                throw new InvalidOperationException($"Payment already exists for booking ID {payment.BookingId}.");
            }

            // Validate amount
            if (payment.Amount <= 0)
            {
                throw new ArgumentException("Payment amount must be greater than zero.");
            }

            // Set default values
            payment.PaymentDate = DateTime.UtcNow;
            payment.Status = PaymentStatus.Pending;

            return await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentAsync(Payment payment)
        {
            var existingPayment = await _paymentRepository.GetByIdAsync(payment.PaymentId);
            if (existingPayment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {payment.PaymentId} not found.");
            }

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeletePaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }

            // Business rule: Only allow deletion of failed or pending payments
            if (payment.Status == PaymentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot delete completed payments.");
            }

            await _paymentRepository.DeleteAsync(paymentId);
        }

        // payment lookup
        public async Task<Payment?> GetPaymentByBookingIdAsync(int bookingId)
        {
            return await _paymentRepository.GetPaymentByBookingIdAsync(bookingId);
        }

        public async Task<Payment?> GetPaymentByTransactionIdAsync(string transactionId)
        {
            return await _paymentRepository.GetPaymentByTransactionIdAsync(transactionId);
        }

        // payment filtering
        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _paymentRepository.GetPaymentsByStatusAsync(status);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByMethodAsync(PaymentMethod paymentMethod)
        {
            return await _paymentRepository.GetPaymentsByMethodAsync(paymentMethod);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _paymentRepository.GetPaymentsByDateRangeAsync(startDate, endDate);
        }

        public async Task<IEnumerable<Payment>> GetFailedPaymentsAsync()
        {
            return await _paymentRepository.GetFailedPaymentsAsync();
        }

        // payment processing
        public async Task<Payment> ProcessPaymentAsync(int bookingId, decimal amount, PaymentMethod paymentMethod)
        {
            // Validate booking exists and is completed
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            if (booking.Status != BookingStatus.Completed)
            {
                throw new InvalidOperationException("Payment can only be processed for completed bookings.");
            }

            // Check if payment already exists
            var existingPayment = await _paymentRepository.GetPaymentByBookingIdAsync(bookingId);
            if (existingPayment != null)
            {
                throw new InvalidOperationException($"Payment already exists for booking ID {bookingId}.");
            }

            // Validate amount matches booking fare
            if (booking.ActualFare.HasValue && amount != booking.ActualFare.Value)
            {
                throw new ArgumentException($"Payment amount ({amount}) does not match booking fare ({booking.ActualFare.Value}).");
            }

            // Create payment
            var payment = new Payment
            {
                BookingId = bookingId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            // Process payment based on method
            switch (paymentMethod)
            {
                case PaymentMethod.Cash:
                    // Cash payment - mark as completed immediately
                    payment.Status = PaymentStatus.Completed;
                    payment.TransactionId = $"CASH-{DateTime.UtcNow:yyyyMMddHHmmss}-{bookingId}";
                    break;

                case PaymentMethod.Card:
                case PaymentMethod.Wallet:
                    // For card/wallet, simulate payment processing
                    // In real scenario, integrate with payment gateway
                    bool paymentSuccess = await SimulatePaymentGatewayAsync(amount, paymentMethod);

                    if (paymentSuccess)
                    {
                        payment.Status = PaymentStatus.Completed;
                        payment.TransactionId = GenerateTransactionId(paymentMethod, bookingId);
                    }
                    else
                    {
                        payment.Status = PaymentStatus.Failed;
                        payment.TransactionId = null;
                    }
                    break;
            }

            return await _paymentRepository.AddAsync(payment);
        }

        public async Task UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }

            await _paymentRepository.UpdatePaymentStatusAsync(paymentId, status);
        }

        public async Task MarkPaymentAsCompletedAsync(int paymentId, string transactionId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }

            if (payment.Status == PaymentStatus.Completed)
            {
                throw new InvalidOperationException("Payment is already completed.");
            }

            payment.Status = PaymentStatus.Completed;
            payment.TransactionId = transactionId;
            payment.PaymentDate = DateTime.UtcNow;

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task MarkPaymentAsFailedAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }

            payment.Status = PaymentStatus.Failed;
            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task RefundPaymentAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }

            if (payment.Status != PaymentStatus.Completed)
            {
                throw new InvalidOperationException("Only completed payments can be refunded.");
            }

            // In real scenario, integrate with payment gateway for refund processing
            // For now, just update status
            payment.Status = PaymentStatus.Refunded;
            await _paymentRepository.UpdateAsync(payment);
        }

        // revenue and statistics
        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _paymentRepository.GetTotalRevenueAsync();
        }

        public async Task<decimal> GetRevenueByMethodAsync(PaymentMethod paymentMethod)
        {
            return await _paymentRepository.GetRevenueByMethodAsync(paymentMethod);
        }

        public async Task<decimal> GetRevenueByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var payments = await _paymentRepository.GetPaymentsByDateRangeAsync(startDate, endDate);
            return payments
                .Where(p => p.Status == PaymentStatus.Completed)
                .Sum(p => p.Amount);
        }

        // helper methods for simulation
        private async Task<bool> SimulatePaymentGatewayAsync(decimal amount, PaymentMethod method)
        {
            // Simulate payment gateway processing
            // In real scenario, call actual payment gateway API
            await Task.Delay(100); // Simulate network delay

            // 95% success rate simulation
            var random = new Random();
            return random.Next(100) < 95;
        }

        private string GenerateTransactionId(PaymentMethod method, int bookingId)
        {
            var prefix = method switch
            {
                PaymentMethod.Card => "CARD",
                PaymentMethod.Wallet => "WALLET",
                PaymentMethod.Cash => "CASH",
                _ => "TXN"
            };

            return $"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmss}-{bookingId}-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
        }
    }
}
