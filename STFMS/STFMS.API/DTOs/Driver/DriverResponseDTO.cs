using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Driver
{
    public class DriverResponseDTO
    {
        public int DriverId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int TotalRides { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedDate { get; set; }
    }
}