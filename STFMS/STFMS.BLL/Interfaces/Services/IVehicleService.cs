using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IVehicleService
    {
        // curd signatures
        Task<Vehicle?> GetVehicleByIdAsync(int vehicleId);
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
        Task UpdateVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(int vehicleId);

        // vehicle lookup signatures
        Task<Vehicle?> GetVehicleByRegistrationNumberAsync(string registrationNumber);
        Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(int driverId);
        Task<Vehicle?> GetVehicleWithMaintenanceHistoryAsync(int vehicleId);

        // vehicle filtering signatures
        Task<IEnumerable<Vehicle>> GetVehiclesByStatusAsync(VehicleStatus status);
        Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType vehicleType);
        Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();

        // vehicle status management signatures
        Task UpdateVehicleStatusAsync(int vehicleId, VehicleStatus status);
        Task SetVehicleActiveAsync(int vehicleId);
        Task SetVehicleInMaintenanceAsync(int vehicleId);
        Task SetVehicleInactiveAsync(int vehicleId);

        // maintenance tracking signatures
        Task<IEnumerable<Vehicle>> GetVehiclesDueForMaintenanceAsync(int daysThreshold);
        Task UpdateLastServiceDateAsync(int vehicleId, DateTime serviceDate);

        // statistics signatures
        Task<int> GetTotalVehiclesCountAsync();
        Task<int> GetActiveVehiclesCountAsync();
    }
}
