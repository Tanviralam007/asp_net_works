using STFMS.BLL.Interfaces.Services;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // curd 
        public async Task<Vehicle?> GetVehicleByIdAsync(int vehicleId)
        {
            return await _vehicleRepository.GetByIdAsync(vehicleId);
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllAsync();
        }

        public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
        {
            // Business validation
            var existingVehicle = await _vehicleRepository.GetVehicleByRegistrationNumberAsync(vehicle.RegistrationNumber);
            if (existingVehicle != null)
            {
                throw new InvalidOperationException($"Registration number '{vehicle.RegistrationNumber}' is already registered.");
            }

            // Set default values
            vehicle.Status = VehicleStatus.Active;
            vehicle.LastServiceDate = DateTime.UtcNow;

            return await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            var existingVehicle = await _vehicleRepository.GetByIdAsync(vehicle.VehicleId);
            if (existingVehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicle.VehicleId} not found.");
            }

            // Check if registration number is being changed and if new registration already exists
            if (existingVehicle.RegistrationNumber != vehicle.RegistrationNumber)
            {
                var vehicleWithRegNumber = await _vehicleRepository.GetVehicleByRegistrationNumberAsync(vehicle.RegistrationNumber);
                if (vehicleWithRegNumber != null)
                {
                    throw new InvalidOperationException($"Registration number '{vehicle.RegistrationNumber}' is already registered.");
                }
            }

            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public async Task DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            // Business rule: Don't allow deletion of vehicles with active bookings
            // This would be checked via navigation properties in a real scenario
            await _vehicleRepository.DeleteAsync(vehicleId);
        }

        // vehicle lookup
        public async Task<Vehicle?> GetVehicleByRegistrationNumberAsync(string registrationNumber)
        {
            return await _vehicleRepository.GetVehicleByRegistrationNumberAsync(registrationNumber);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(int driverId)
        {
            return await _vehicleRepository.GetVehiclesByDriverIdAsync(driverId);
        }

        public async Task<Vehicle?> GetVehicleWithMaintenanceHistoryAsync(int vehicleId)
        {
            return await _vehicleRepository.GetVehicleWithMaintenanceHistoryAsync(vehicleId);
        }

        // vehicle filtering
        public async Task<IEnumerable<Vehicle>> GetVehiclesByStatusAsync(VehicleStatus status)
        {
            return await _vehicleRepository.GetVehiclesByStatusAsync(status);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType vehicleType)
        {
            return await _vehicleRepository.GetVehiclesByTypeAsync(vehicleType);
        }

        public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
        {
            return await _vehicleRepository.GetActiveVehiclesAsync();
        }

        // vehicle status management
        public async Task UpdateVehicleStatusAsync(int vehicleId, VehicleStatus status)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            await _vehicleRepository.UpdateVehicleStatusAsync(vehicleId, status);
        }

        public async Task SetVehicleActiveAsync(int vehicleId)
        {
            await UpdateVehicleStatusAsync(vehicleId, VehicleStatus.Active);
        }

        public async Task SetVehicleInMaintenanceAsync(int vehicleId)
        {
            await UpdateVehicleStatusAsync(vehicleId, VehicleStatus.Maintenance);
        }

        public async Task SetVehicleInactiveAsync(int vehicleId)
        {
            await UpdateVehicleStatusAsync(vehicleId, VehicleStatus.Inactive);
        }

        // maintenance tracking
        public async Task<IEnumerable<Vehicle>> GetVehiclesDueForMaintenanceAsync(int daysThreshold)
        {
            return await _vehicleRepository.GetVehiclesDueForMaintenanceAsync(daysThreshold);
        }

        public async Task UpdateLastServiceDateAsync(int vehicleId, DateTime serviceDate)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            await _vehicleRepository.UpdateLastServiceDateAsync(vehicleId, serviceDate);
        }

        // statistics
        public async Task<int> GetTotalVehiclesCountAsync()
        {
            return await _vehicleRepository.CountAsync();
        }

        public async Task<int> GetActiveVehiclesCountAsync()
        {
            return await _vehicleRepository.GetTotalActiveVehiclesCountAsync();
        }
    }
}