using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.DAL.Repositories
{
    public class MaintenanceRepository : GenericRepository<Maintenance>, IMaintenanceRepository
    {
        public MaintenanceRepository(AppDbContext context) 
            : base(context)
        {

        }

        public async Task<IEnumerable<Maintenance>> GetMaintenanceByVehicleIdAsync(int vehicleId)
        {
            return await _dbSet
                .Where(m => m.VehicleId == vehicleId)
                .OrderByDescending(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenanceByStatusAsync(MaintenanceStatus status)
        {
            return await _dbSet
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Driver)
                        .ThenInclude(d => d.User)
                .Where(m => m.Status == status)
                .OrderBy(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenanceByTypeAsync(MaintenanceType maintenanceType)
        {
            return await _dbSet
                .Include(m => m.Vehicle)
                .Where(m => m.MaintenanceType == maintenanceType)
                .OrderByDescending(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetUpcomingMaintenanceAsync(int daysAhead)
        {
            var futureDate = DateTime.UtcNow.AddDays(daysAhead);
            return await _dbSet
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Driver)
                        .ThenInclude(d => d.User)
                .Where(m => m.Status == MaintenanceStatus.Scheduled &&
                           m.ScheduledDate >= DateTime.UtcNow &&
                           m.ScheduledDate <= futureDate)
                .OrderBy(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetOverdueMaintenanceAsync()
        {
            return await _dbSet
                .Include(m => m.Vehicle)
                    .ThenInclude(v => v.Driver)
                        .ThenInclude(d => d.User)
                .Where(m => m.Status == MaintenanceStatus.Scheduled && m.ScheduledDate < DateTime.UtcNow)
                .OrderBy(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(m => m.Vehicle)
                .Where(m => m.ScheduledDate >= startDate && m.ScheduledDate <= endDate)
                .OrderBy(m => m.ScheduledDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId)
        {
            return await _dbSet
                .Where(m => m.VehicleId == vehicleId && m.Status == MaintenanceStatus.Completed)
                .SumAsync(m => m.Cost);
        }

        public async Task<decimal> GetTotalMaintenanceCostAsync()
        {
            return await _dbSet
                .Where(m => m.Status == MaintenanceStatus.Completed)
                .SumAsync(m => m.Cost);
        }

        public async Task UpdateMaintenanceStatusAsync(int maintenanceId, MaintenanceStatus status)
        {
            var maintenance = await _dbSet.FindAsync(maintenanceId);
            if (maintenance != null)
            {
                maintenance.Status = status;

                if (status == MaintenanceStatus.Completed && maintenance.CompletedDate == null)
                {
                    maintenance.CompletedDate = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}
