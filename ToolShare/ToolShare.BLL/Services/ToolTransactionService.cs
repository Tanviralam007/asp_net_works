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
    public class ToolTransactionService : IToolTransactionService
    {
        private readonly IToolTransactionRepository _transactionRepo;
        private readonly IBorrowRequestRepository _requestRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IToolRepository _toolRepo;

        public ToolTransactionService(
            IToolTransactionRepository transactionRepo,
            IBorrowRequestRepository requestRepo,
            IPaymentRepository paymentRepo,
            IToolRepository toolRepo)
        {
            _transactionRepo = transactionRepo;
            _requestRepo = requestRepo;
            _paymentRepo = paymentRepo;
            _toolRepo = toolRepo;
        }

        public async Task<ToolTransaction?> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepo.GetByIdAsync(id);
        }

        public async Task<ToolTransaction?> GetTransactionByRequestIdAsync(int borrowRequestId)
        {
            return await _transactionRepo.GetByBorrowRequestIdAsync(borrowRequestId);
        }

        public async Task<IEnumerable<ToolTransaction>> GetActiveTransactionsAsync()
        {
            return await _transactionRepo.GetActiveTransactionsAsync();
        }

        public async Task<IEnumerable<ToolTransaction>> GetOverdueTransactionsAsync()
        {
            return await _transactionRepo.GetOverdueTransactionsAsync();
        }

        public async Task<IEnumerable<ToolTransaction>> GetTransactionsByBorrowerAsync(int borrowerId)
        {
            return await _transactionRepo.GetTransactionsByBorrowerAsync(borrowerId);
        }

        public async Task<IEnumerable<ToolTransaction>> GetTransactionsByOwnerAsync(int ownerId)
        {
            return await _transactionRepo.GetTransactionsByOwnerAsync(ownerId);
        }

        public async Task<ToolTransaction> ConfirmHandoverAsync(int requestId, int ownerId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null)
                throw new KeyNotFoundException("Request not found");

            var tool = await _toolRepo.GetByIdAsync(request.ToolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            // Authorization
            if (tool.OwnerId != ownerId)
                throw new UnauthorizedAccessException("Only the tool owner can confirm handover");

            // Validate request status
            if ((byte)request.Status != 2)
                throw new InvalidOperationException("Request must be approved");

            // Check payment
            var payment = await _paymentRepo.GetByBorrowRequestIdAsync(requestId);
            if (payment == null)
                throw new InvalidOperationException("Payment required before handover");

            // Check if transaction already exists
            var existingTransaction = await _transactionRepo.GetByBorrowRequestIdAsync(requestId);
            if (existingTransaction != null)
                throw new InvalidOperationException("Transaction already exists for this request");

            var transaction = new ToolTransaction
            {
                BorrowRequestId = requestId,
                PaymentId = payment.Id,
                HandoverDate = DateTime.Now,
                ExpectedReturnDate = request.EndDate,
                ReturnDate = null,
                LateDays = 0,
                FineAmount = 0,
                Status = (TransactionStatus)1 // InProgress
            };

            return await _transactionRepo.AddAsync(transaction);
        }

        public async Task<ToolTransaction> ProcessReturnAsync(int transactionId, int ownerId)
        {
            var transaction = await _transactionRepo.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new KeyNotFoundException("Transaction not found");

            var request = await _requestRepo.GetByIdAsync(transaction.BorrowRequestId);
            if (request == null)
                throw new KeyNotFoundException("Request not found");

            var tool = await _toolRepo.GetByIdAsync(request.ToolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            // Authorization
            if (tool.OwnerId != ownerId)
                throw new UnauthorizedAccessException("Only the tool owner can process return");

            // Validate transaction status
            if ((byte)transaction.Status != 1)
                throw new InvalidOperationException("Only active transactions can be returned");

            transaction.ReturnDate = DateTime.Now;

            // Calculate late days and fine
            if (transaction.ReturnDate > transaction.ExpectedReturnDate)
            {
                transaction.LateDays = (transaction.ReturnDate.Value - transaction.ExpectedReturnDate).Days;
                transaction.FineAmount = await CalculateFineAsync(
                    transaction.ExpectedReturnDate,
                    transaction.ReturnDate.Value,
                    tool.DailyRate);
            }

            transaction.Status = (TransactionStatus)2; // Completed

            // Make tool available again
            tool.IsAvailable = true;
            await _toolRepo.UpdateAsync(tool);

            return await _transactionRepo.UpdateAsync(transaction);
        }

        public Task<decimal> CalculateFineAsync(DateTime expectedReturnDate, DateTime actualReturnDate, decimal dailyRate)
        {
            if (actualReturnDate <= expectedReturnDate)
                return Task.FromResult(0m);

            var lateDays = (actualReturnDate - expectedReturnDate).Days;
            var finePerDay = dailyRate * 0.5m; // 50% of daily rate as fine

            return Task.FromResult(lateDays * finePerDay);
        }
    }
}
