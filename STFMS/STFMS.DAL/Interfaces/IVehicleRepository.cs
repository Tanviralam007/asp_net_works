using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(int driverId);
        Task<IEnumerable<Vehicle>> GetVehiclesByStatusAsync(VehicleStatus status);
        Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType vehicleType);
        Task<Vehicle?> GetVehicleByRegistrationNumberAsync(string registrationNumber);
        Task<IEnumerable<Vehicle>> GetVehiclesDueForMaintenanceAsync(int daysThreshold);
        Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
        Task<Vehicle?> GetVehicleWithMaintenanceHistoryAsync(int vehicleId);
        Task<int> GetTotalActiveVehiclesCountAsync();
        Task UpdateVehicleStatusAsync(int vehicleId, VehicleStatus status);
        Task UpdateLastServiceDateAsync(int vehicleId, DateTime serviceDate);
    }
}
