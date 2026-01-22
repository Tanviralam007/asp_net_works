using STFMS.API.DTOs.User;
using STFMS.API.DTOs.Driver;
using STFMS.API.DTOs.Vehicle;
using STFMS.API.DTOs.Payment;
using STFMS.API.DTOs.Feedback;

namespace STFMS.API.DTOs.Booking
{
    public class BookingWithDetailsResponseDTO : BookingResponseDTO
    {
        public UserResponseDTO? User { get; set; }
        public DriverWithDetailsResponseDTO? Driver { get; set; }
        public VehicleResponseDTO? Vehicle { get; set; }
        public PaymentResponseDTO? Payment { get; set; }
        public FeedbackResponseDTO? Feedback { get; set; }
    }
}
