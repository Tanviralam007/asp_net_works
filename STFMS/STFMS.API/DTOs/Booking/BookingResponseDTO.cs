using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Booking
{
    public class BookingResponseDTO
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int? DriverId { get; set; }
        public int? VehicleId { get; set; }
        public string PickupLocation { get; set; } = string.Empty;
        public string DropoffLocation { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public DateTime? PickupTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal EstimatedFare { get; set; }
        public decimal? ActualFare { get; set; }
        public string ServiceType { get; set; } = string.Empty;
    }
}
