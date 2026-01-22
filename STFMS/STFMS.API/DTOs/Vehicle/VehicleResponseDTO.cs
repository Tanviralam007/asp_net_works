using STFMS.DAL.Entities;

namespace STFMS.API.DTOs.Vehicle
{
    public class VehicleResponseDTO
    {
        public int VehicleId { get; set; }
        public int DriverId { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public VehicleType VehicleType { get; set; }
        public int Capacity { get; set; }
        public VehicleStatus Status { get; set; }
        public DateTime LastServiceDate { get; set; }
    }
}
