using STFMS.API.DTOs.Driver;
using STFMS.API.DTOs.Booking;

namespace STFMS.API.DTOs.User
{
    public class UserWithDetailsResponseDTO : UserResponseDTO
    {
        public DriverResponseDTO? Driver { get; set; }
        public List<BookingResponseDTO>? Bookings { get; set; }
    }
}
