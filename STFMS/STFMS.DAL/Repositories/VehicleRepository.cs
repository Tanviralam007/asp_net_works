using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.DAL.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByDriverIdAsync(int driverId)
        {
            return await _dbSet
                .Where(v => v.DriverId == driverId)
                .OrderBy(v => v.RegistrationNumber)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByStatusAsync(VehicleStatus status)
        {
            return await _dbSet
                .Include(v => v.Driver)
                    .ThenInclude(d => d.User)
                .Where(v => v.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesByTypeAsync(VehicleType vehicleType)
        {
            return await _dbSet
                .Include(v => v.Driver)
                    .ThenInclude(d => d.User)
                .Where(v => v.VehicleType == vehicleType)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleByRegistrationNumberAsync(string registrationNumber)
        {
            return await _dbSet
                .Include(v => v.Driver)
                    .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesDueForMaintenanceAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(-daysThreshold);
            return await _dbSet
                .Include(v => v.Driver)
                    .ThenInclude(d => d.User)
                .Where(v => v.LastServiceDate <= thresholdDate && v.Status == VehicleStatus.Active)
                .OrderBy(v => v.LastServiceDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
        {
            return await _dbSet
                .Where(v => v.Status == VehicleStatus.Active)
                .ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleWithMaintenanceHistoryAsync(int vehicleId)
        {
            return await _dbSet
                .Include(v => v.MaintenanceRecords)
                .Include(v => v.Driver)
                    .ThenInclude(d => d.User)
                .FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
        }

        public async Task<int> GetTotalActiveVehiclesCountAsync()
        {
            return await _dbSet
                .CountAsync(v => v.Status == VehicleStatus.Active);
        }

        public async Task UpdateVehicleStatusAsync(int vehicleId, VehicleStatus status)
        {
            var vehicle = await _dbSet.FindAsync(vehicleId);
            if (vehicle != null)
            {
                vehicle.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateLastServiceDateAsync(int vehicleId, DateTime serviceDate)
        {
            var vehicle = await _dbSet.FindAsync(vehicleId);
            if (vehicle != null)
            {
                vehicle.LastServiceDate = serviceDate;
                await _context.SaveChangesAsync();
            }
        }
    }
}
