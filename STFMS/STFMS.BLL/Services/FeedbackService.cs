using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using STFMS.BLL.Interfaces.Services;

namespace STFMS.BLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IDriverRepository _driverRepository;

        public FeedbackService(
            IFeedbackRepository feedbackRepository,
            IBookingRepository bookingRepository,
            IDriverRepository driverRepository)
        {
            _feedbackRepository = feedbackRepository;
            _bookingRepository = bookingRepository;
            _driverRepository = driverRepository;
        }

        // curd
        public async Task<Feedback?> GetFeedbackByIdAsync(int feedbackId)
        {
            return await _feedbackRepository.GetByIdAsync(feedbackId);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await _feedbackRepository.GetAllAsync();
        }

        public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
        {
            var booking = await _bookingRepository.GetByIdAsync(feedback.BookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {feedback.BookingId} not found.");
            }

            // check if feedback already exists for this booking
            var existingFeedback = await _feedbackRepository.GetFeedbackByBookingIdAsync(feedback.BookingId);
            if (existingFeedback != null)
            {
                throw new InvalidOperationException($"Feedback already exists for booking ID {feedback.BookingId}.");
            }

            // validate booking is completed
            if (booking.Status != BookingStatus.Completed)
            {
                throw new InvalidOperationException("Feedback can only be submitted for completed bookings.");
            }

            // validate rating range
            if (feedback.Rating < 1 || feedback.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            // validate user matches booking user
            if (feedback.UserId != booking.UserId)
            {
                throw new InvalidOperationException("Feedback can only be submitted by the booking customer.");
            }

            // set submitted date
            feedback.SubmittedDate = DateTime.UtcNow;

            var createdFeedback = await _feedbackRepository.AddAsync(feedback);

            // update driver rating after feedback submission
            if (booking.DriverId.HasValue)
            {
                await UpdateDriverRatingAfterFeedbackAsync(booking.DriverId.Value);
            }

            return createdFeedback;
        }

        public async Task UpdateFeedbackAsync(Feedback feedback)
        {
            var existingFeedback = await _feedbackRepository.GetByIdAsync(feedback.FeedbackId);
            if (existingFeedback == null)
            {
                throw new KeyNotFoundException($"Feedback with ID {feedback.FeedbackId} not found.");
            }

            // validate rating range
            if (feedback.Rating < 1 || feedback.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            await _feedbackRepository.UpdateAsync(feedback);

            // recalculate driver rating
            var booking = await _bookingRepository.GetByIdAsync(feedback.BookingId);
            if (booking?.DriverId.HasValue == true)
            {
                await UpdateDriverRatingAfterFeedbackAsync(booking.DriverId.Value);
            }
        }

        public async Task DeleteFeedbackAsync(int feedbackId)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null)
            {
                throw new KeyNotFoundException($"Feedback with ID {feedbackId} not found.");
            }

            await _feedbackRepository.DeleteAsync(feedbackId);

            // recalculate driver rating after feedback deletion
            var booking = await _bookingRepository.GetByIdAsync(feedback.BookingId);
            if (booking?.DriverId.HasValue == true)
            {
                await UpdateDriverRatingAfterFeedbackAsync(booking.DriverId.Value);
            }
        }

        // feedback lookup
        public async Task<Feedback?> GetFeedbackByBookingIdAsync(int bookingId)
        {
            return await _feedbackRepository.GetFeedbackByBookingIdAsync(bookingId);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByUserIdAsync(int userId)
        {
            return await _feedbackRepository.GetFeedbacksByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByDriverIdAsync(int driverId)
        {
            return await _feedbackRepository.GetFeedbacksByDriverIdAsync(driverId);
        }

        // feedback filtering
        public async Task<IEnumerable<Feedback>> GetFeedbacksByRatingAsync(int rating)
        {
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            return await _feedbackRepository.GetFeedbacksByRatingAsync(rating);
        }

        public async Task<IEnumerable<Feedback>> GetLowRatedFeedbacksAsync(int threshold)
        {
            if (threshold < 1 || threshold > 5)
            {
                throw new ArgumentException("Threshold must be between 1 and 5.");
            }

            return await _feedbackRepository.GetLowRatedFeedbacksAsync(threshold);
        }

        public async Task<IEnumerable<Feedback>> GetRecentFeedbacksAsync(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than zero.");
            }

            return await _feedbackRepository.GetRecentFeedbacksAsync(count);
        }

        // feedback submission
        public async Task<Feedback> SubmitFeedbackAsync(int bookingId, int userId, int rating, string? comments)
        {
            // validate booking exists
            var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
            if (booking == null)
            {
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
            }

            // check if feedback already exists
            var existingFeedback = await _feedbackRepository.GetFeedbackByBookingIdAsync(bookingId);
            if (existingFeedback != null)
            {
                throw new InvalidOperationException($"Feedback already exists for booking ID {bookingId}.");
            }

            // validate booking is completed
            if (booking.Status != BookingStatus.Completed)
            {
                throw new InvalidOperationException("Feedback can only be submitted for completed bookings.");
            }

            // validate user matches booking
            if (booking.UserId != userId)
            {
                throw new InvalidOperationException("Only the booking customer can submit feedback.");
            }

            // validate rating
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            // create feedback
            var feedback = new Feedback
            {
                BookingId = bookingId,
                UserId = userId,
                Rating = rating,
                Comments = comments,
                SubmittedDate = DateTime.UtcNow
            };

            var createdFeedback = await _feedbackRepository.AddAsync(feedback);

            // update driver rating
            if (booking.DriverId.HasValue)
            {
                await UpdateDriverRatingAfterFeedbackAsync(booking.DriverId.Value);
            }

            return createdFeedback;
        }

        // rating calculation
        public async Task<decimal> GetAverageRatingForDriverAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            return await _feedbackRepository.GetAverageRatingForDriverAsync(driverId);
        }

        public async Task UpdateDriverRatingAfterFeedbackAsync(int driverId)
        {
            var driver = await _driverRepository.GetByIdAsync(driverId);
            if (driver == null)
            {
                throw new KeyNotFoundException($"Driver with ID {driverId} not found.");
            }

            // calculate new average rating from all feedbacks
            var averageRating = await _feedbackRepository.GetAverageRatingForDriverAsync(driverId);

            // update driver rating (0 if no feedbacks exist)
            await _driverRepository.UpdateDriverRatingAsync(driverId, averageRating);
        }

        // statistics
        public async Task<int> GetTotalFeedbacksCountAsync()
        {
            return await _feedbackRepository.CountAsync();
        }

        public async Task<decimal> GetOverallAverageRatingAsync()
        {
            var allFeedbacks = await _feedbackRepository.GetAllAsync();

            if (!allFeedbacks.Any())
            {
                return 0m;
            }

            return (decimal)allFeedbacks.Average(f => f.Rating);
        }
    }
}
