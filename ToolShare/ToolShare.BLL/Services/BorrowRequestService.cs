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
    public class BorrowRequestService : IBorrowRequestService
    {
        private readonly IBorrowRequestRepository _requestRepo;
        private readonly IToolRepository _toolRepo;
        private readonly IUserRepository _userRepo;

        public BorrowRequestService(
            IBorrowRequestRepository requestRepo,
            IToolRepository toolRepo,
            IUserRepository userRepo)
        {
            _requestRepo = requestRepo;
            _toolRepo = toolRepo;
            _userRepo = userRepo;
        }

        public async Task<IEnumerable<BorrowRequest>> GetAllRequestsAsync()
        {
            return await _requestRepo.GetAllAsync();
        }

        public async Task<BorrowRequest?> GetRequestByIdAsync(int id)
        {
            return await _requestRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByBorrowerAsync(int borrowerId)
        {
            return await _requestRepo.GetRequestsByBorrowerAsync(borrowerId);
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByToolAsync(int toolId)
        {
            return await _requestRepo.GetRequestsByToolAsync(toolId);
        }

        public async Task<IEnumerable<BorrowRequest>> GetRequestsByOwnerAsync(int ownerId)
        {
            return await _requestRepo.GetRequestsByOwnerAsync(ownerId);
        }

        public async Task<IEnumerable<BorrowRequest>> GetPendingRequestsForOwnerAsync(int ownerId)
        {
            return await _requestRepo.GetPendingRequestsForOwnerAsync(ownerId);
        }

        public async Task<BorrowRequest> CreateRequestAsync(BorrowRequest request, int borrowerId)
        {
            // Validate borrower
            var borrower = await _userRepo.GetByIdAsync(borrowerId);
            if (borrower == null)
                throw new KeyNotFoundException("Borrower not found");

            if ((byte)borrower.Role != 0) // Borrower role = 0
                throw new UnauthorizedAccessException("Only borrowers can create requests");

            if (borrower.IsBlocked)
                throw new InvalidOperationException("Blocked users cannot create requests");

            // Validate tool
            var tool = await _toolRepo.GetByIdAsync(request.ToolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            if (!tool.IsAvailable)
                throw new InvalidOperationException("Tool is not available");

            // Validate dates
            if (request.StartDate < DateTime.Now.Date)
                throw new ArgumentException("Start date cannot be in the past");

            if (request.EndDate <= request.StartDate)
                throw new ArgumentException("End date must be after start date");

            // Check for overlapping requests
            if (await _requestRepo.HasOverlappingRequestsAsync(request.ToolId, request.StartDate, request.EndDate))
                throw new InvalidOperationException("Tool is already booked for the selected dates");

            // Set request properties
            request.BorrowerId = borrowerId;
            request.RequestDate = DateTime.Now;
            request.Status = RequestStatus.Pending; // Pending
            request.ApprovalDate = null;

            return await _requestRepo.AddAsync(request);
        }

        public async Task<bool> CancelRequestAsync(int requestId, int borrowerId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null)
                throw new KeyNotFoundException("Request not found");

            // Authorization: Only borrower can cancel their own request
            if (request.BorrowerId != borrowerId)
                throw new UnauthorizedAccessException("You can only cancel your own requests");

            // Can only cancel pending requests
            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be cancelled");

            return await _requestRepo.DeleteAsync(requestId);
        }

        public async Task<BorrowRequest> ApproveRequestAsync(int requestId, int ownerId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null)
                throw new KeyNotFoundException("Request not found");

            // Get tool to verify ownership
            var tool = await _toolRepo.GetByIdAsync(request.ToolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            // Authorization: Only tool owner can approve
            if (tool.OwnerId != ownerId)
                throw new UnauthorizedAccessException("Only the tool owner can approve requests");

            // Can only approve pending requests
            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be approved");

            request.Status = RequestStatus.Approved; // Approved
            request.ApprovalDate = DateTime.Now;

            return await _requestRepo.UpdateAsync(request);
        }

        public async Task<BorrowRequest> RejectRequestAsync(int requestId, int ownerId)
        {
            var request = await _requestRepo.GetByIdAsync(requestId);
            if (request == null)
                throw new KeyNotFoundException("Request not found");

            var tool = await _toolRepo.GetByIdAsync(request.ToolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            if (tool.OwnerId != ownerId)
                throw new UnauthorizedAccessException("Only the tool owner can reject requests");

            if (request.Status != RequestStatus.Pending)
                throw new InvalidOperationException("Only pending requests can be rejected");

            request.Status = RequestStatus.Rejected; // Rejected
            request.ApprovalDate = DateTime.Now;

            return await _requestRepo.UpdateAsync(request);
        }

        public async Task<decimal> CalculateRentalCostAsync(int toolId, DateTime startDate, DateTime endDate)
        {
            var tool = await _toolRepo.GetByIdAsync(toolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            var days = (endDate - startDate).Days;
            if (days <= 0)
                throw new ArgumentException("Invalid date range");

            return tool.DailyRate * days;
        }
    }
}
