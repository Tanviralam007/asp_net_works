using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Booking
{
    public class BookingSearchRequest
    {
        public int? UserId { get; set; }
        public int? DriverId { get; set; }
        public BookingStatus? Status { get; set; }
        public ServiceType? ServiceType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinFare { get; set; }
        public decimal? MaxFare { get; set; }
        public string? PickupLocation { get; set; }
        public string? DropoffLocation { get; set; }
    }
}
