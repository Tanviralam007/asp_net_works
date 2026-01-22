using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.BLL.Interfaces.Services;
using ToolShare.DAL.Entities;
using ToolShare.DAL.Interfaces;

namespace ToolShare.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IBorrowRequestRepository _requestRepo;
        private readonly IUserRepository _userRepo;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IBorrowRequestRepository requestRepo,
            IUserRepository userRepo)
        {
            _paymentRepo = paymentRepo;
            _requestRepo = requestRepo;
            _userRepo = userRepo;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepo.GetByIdAsync(id);
        }

        public async Task<Payment?> GetPaymentByRequestIdAsync(int borrowRequestId)
        {
            return await _paymentRepo.GetByBorrowRequestIdAsync(borrowRequestId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId)
        {
            return await _paymentRepo.GetPaymentsByUserAsync(userId);
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment, int borrowerId)
        {
            // Validate borrower
            var borrower = await _userRepo.GetByIdAsync(borrowerId);
            if (borrower == null)
                throw new KeyNotFoundException("Borrower not found");

            if (borrower.IsBlocked)
                throw new InvalidOperationException("Blocked users cannot make payments");

            // Validate request
            var request = await _requestRepo.GetByIdAsync(payment.BorrowRequestId);
            if (request == null)
                throw new KeyNotFoundException("Borrow request not found");

            if (request.BorrowerId != borrowerId)
                throw new UnauthorizedAccessException("You can only pay for your own requests");

            if ((byte)request.Status != 2) // Must be approved
                throw new InvalidOperationException("Only approved requests can be paid");

            // Check if already paid
            var existingPayment = await _paymentRepo.GetByBorrowRequestIdAsync(payment.BorrowRequestId);
            if (existingPayment != null)
                throw new InvalidOperationException("This request has already been paid");

            // Validate payment amount
            if (payment.Amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero");

            payment.PaymentDate = DateTime.Now;

            return await _paymentRepo.AddAsync(payment);
        }

        public async Task<decimal> GetTotalPaymentsByOwnerAsync(int ownerId)
        {
            return await _paymentRepo.GetTotalPaymentsByOwnerAsync(ownerId);
        }
    }
}
