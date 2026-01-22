using STFMS.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IMaintenanceService
    {
        // curd signatures
        Task<Maintenance?> GetMaintenanceByIdAsync(int maintenanceId);
        Task<IEnumerable<Maintenance>> GetAllMaintenanceAsync();
        Task<Maintenance> CreateMaintenanceAsync(Maintenance maintenance);
        Task UpdateMaintenanceAsync(Maintenance maintenance);
        Task DeleteMaintenanceAsync(int maintenanceId);

        // maintenance lookup signatures
        Task<IEnumerable<Maintenance>> GetMaintenanceByVehicleIdAsync(int vehicleId);

        // maintenance filtering signatures
        Task<IEnumerable<Maintenance>> GetMaintenanceByStatusAsync(MaintenanceStatus status);
        Task<IEnumerable<Maintenance>> GetMaintenanceByTypeAsync(MaintenanceType maintenanceType);
        Task<IEnumerable<Maintenance>> GetUpcomingMaintenanceAsync(int daysAhead);
        Task<IEnumerable<Maintenance>> GetOverdueMaintenanceAsync();
        Task<IEnumerable<Maintenance>> GetMaintenanceByDateRangeAsync(DateTime startDate, DateTime endDate);

        // maintenance scheduling signatures
        Task<Maintenance> ScheduleMaintenanceAsync(int vehicleId, MaintenanceType type, string description, decimal cost, DateTime scheduledDate);
        Task UpdateMaintenanceStatusAsync(int maintenanceId, MaintenanceStatus status);
        Task CompleteMaintenanceAsync(int maintenanceId);

        // alerts & notifications signatures
        Task<IEnumerable<Maintenance>> GetMaintenanceAlertsAsync();
        Task SendMaintenanceRemindersAsync();

        // cost analysis signatures
        Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId);
        Task<decimal> GetTotalMaintenanceCostAsync();
        Task<decimal> GetMaintenanceCostByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
