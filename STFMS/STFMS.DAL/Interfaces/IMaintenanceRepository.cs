using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface IMaintenanceRepository : IGenericRepository<Maintenance>
    {
        Task<IEnumerable<Maintenance>> GetMaintenanceByVehicleIdAsync(int vehicleId);
        Task<IEnumerable<Maintenance>> GetMaintenanceByStatusAsync(MaintenanceStatus status);
        Task<IEnumerable<Maintenance>> GetMaintenanceByTypeAsync(MaintenanceType maintenanceType);
        Task<IEnumerable<Maintenance>> GetUpcomingMaintenanceAsync(int daysAhead);
        Task<IEnumerable<Maintenance>> GetOverdueMaintenanceAsync();
        Task<IEnumerable<Maintenance>> GetMaintenanceByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId);
        Task<decimal> GetTotalMaintenanceCostAsync();
        Task UpdateMaintenanceStatusAsync(int maintenanceId, MaintenanceStatus status);
    }
}
