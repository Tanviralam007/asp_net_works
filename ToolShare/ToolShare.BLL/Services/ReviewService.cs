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
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IToolRepository _toolRepo;
        private readonly IUserRepository _userRepo;
        private readonly IToolTransactionRepository _transactionRepo;

        public ReviewService(
            IReviewRepository reviewRepo,
            IToolRepository toolRepo,
            IUserRepository userRepo,
            IToolTransactionRepository transactionRepo)
        {
            _reviewRepo = reviewRepo;
            _toolRepo = toolRepo;
            _userRepo = userRepo;
            _transactionRepo = transactionRepo;
        }

        public async Task<IEnumerable<Review>> GetReviewsByToolAsync(int toolId)
        {
            return await _reviewRepo.GetReviewsByToolAsync(toolId);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(int userId)
        {
            return await _reviewRepo.GetReviewsByUserAsync(userId);
        }

        public async Task<Review> CreateReviewAsync(Review review, int reviewerId)
        {
            // Validate reviewer
            var reviewer = await _userRepo.GetByIdAsync(reviewerId);
            if (reviewer == null)
                throw new KeyNotFoundException("Reviewer not found");

            if (reviewer.IsBlocked)
                throw new InvalidOperationException("Blocked users cannot create reviews");

            // Validate tool
            var tool = await _toolRepo.GetByIdAsync(review.ToolId);
            if (tool == null)
                throw new KeyNotFoundException("Tool not found");

            // Validate rating
            if (review.Rating < 1 || review.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            // Check if user has completed a transaction with this tool
            var transactions = await _transactionRepo.GetTransactionsByBorrowerAsync(reviewerId);
            var hasCompleted = transactions.Any(t =>
                t.BorrowRequest.ToolId == review.ToolId &&
                (byte)t.Status == 2); // Completed

            if (!hasCompleted)
                throw new InvalidOperationException("You can only review tools you have borrowed and returned");

            // Check if already reviewed
            if (await _reviewRepo.HasUserReviewedToolAsync(reviewerId, review.ToolId))
                throw new InvalidOperationException("You have already reviewed this tool");

            review.UserId = reviewerId;
            review.ReviewDate = DateTime.Now;

            return await _reviewRepo.AddAsync(review);
        }

        public async Task<bool> DeleteReviewAsync(int reviewId, int userId, byte userRole)
        {
            var review = await _reviewRepo.GetByIdAsync(reviewId);
            if (review == null)
                throw new KeyNotFoundException("Review not found");

            // Authorization: Admin or review owner can delete
            if (userRole == 2) // Admin
            {
                return await _reviewRepo.DeleteAsync(reviewId);
            }
            else if (review.UserId == userId) // Review owner
            {
                return await _reviewRepo.DeleteAsync(reviewId);
            }
            else
            {
                throw new UnauthorizedAccessException("You don't have permission to delete this review");
            }
        }

        public async Task<double> GetAverageRatingForToolAsync(int toolId)
        {
            return await _reviewRepo.GetAverageRatingForToolAsync(toolId);
        }
    }
}
