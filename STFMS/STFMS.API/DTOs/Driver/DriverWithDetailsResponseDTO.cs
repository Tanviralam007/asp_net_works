using STFMS.API.DTOs.User;
using STFMS.API.DTOs.Vehicle;

namespace STFMS.API.DTOs.Driver
{
    public class DriverWithDetailsResponseDTO : DriverResponseDTO
    {
        public UserResponseDTO? User { get; set; }
        public List<VehicleResponseDTO>? Vehicles { get; set; }
        public decimal? TotalRevenue { get; set; }
        public decimal? AverageRating { get; set; }
    }
}
