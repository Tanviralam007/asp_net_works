using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using STFMS.BLL.Interfaces.Services;

namespace STFMS.BLL.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly IMaintenanceRepository _maintenanceRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public MaintenanceService(
            IMaintenanceRepository maintenanceRepository,
            IVehicleRepository vehicleRepository)
        {
            _maintenanceRepository = maintenanceRepository;
            _vehicleRepository = vehicleRepository;
        }

        // curd
        public async Task<Maintenance?> GetMaintenanceByIdAsync(int maintenanceId)
        {
            return await _maintenanceRepository.GetByIdAsync(maintenanceId);
        }

        public async Task<IEnumerable<Maintenance>> GetAllMaintenanceAsync()
        {
            return await _maintenanceRepository.GetAllAsync();
        }

        public async Task<Maintenance> CreateMaintenanceAsync(Maintenance maintenance)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(maintenance.VehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {maintenance.VehicleId} not found.");
            }

            // validate cost
            if (maintenance.Cost < 0)
            {
                throw new ArgumentException("Maintenance cost cannot be negative.");
            }

            // validate scheduled date
            if (maintenance.ScheduledDate < DateTime.UtcNow.AddDays(-1))
            {
                throw new ArgumentException("Scheduled date cannot be in the past.");
            }

            // set default values
            maintenance.Status = MaintenanceStatus.Scheduled;
            maintenance.CompletedDate = null;

            return await _maintenanceRepository.AddAsync(maintenance);
        }

        public async Task UpdateMaintenanceAsync(Maintenance maintenance)
        {
            var existingMaintenance = await _maintenanceRepository.GetByIdAsync(maintenance.MaintenanceId);
            if (existingMaintenance == null)
            {
                throw new KeyNotFoundException($"Maintenance with ID {maintenance.MaintenanceId} not found.");
            }

            // validate cost
            if (maintenance.Cost < 0)
            {
                throw new ArgumentException("Maintenance cost cannot be negative.");
            }

            await _maintenanceRepository.UpdateAsync(maintenance);
        }

        public async Task DeleteMaintenanceAsync(int maintenanceId)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            if (maintenance == null)
            {
                throw new KeyNotFoundException($"Maintenance with ID {maintenanceId} not found.");
            }

            // only allow deletion of scheduled or completed maintenance
            if (maintenance.Status == MaintenanceStatus.InProgress)
            {
                throw new InvalidOperationException("Cannot delete in-progress maintenance.");
            }

            await _maintenanceRepository.DeleteAsync(maintenanceId);
        }

        // Maintenance lookup
        public async Task<IEnumerable<Maintenance>> GetMaintenanceByVehicleIdAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            return await _maintenanceRepository.GetMaintenanceByVehicleIdAsync(vehicleId);
        }

        //Maintenance filtering
        public async Task<IEnumerable<Maintenance>> GetMaintenanceByStatusAsync(MaintenanceStatus status)
        {
            return await _maintenanceRepository.GetMaintenanceByStatusAsync(status);
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenanceByTypeAsync(MaintenanceType maintenanceType)
        {
            return await _maintenanceRepository.GetMaintenanceByTypeAsync(maintenanceType);
        }

        public async Task<IEnumerable<Maintenance>> GetUpcomingMaintenanceAsync(int daysAhead)
        {
            if (daysAhead < 0)
            {
                throw new ArgumentException("Days ahead cannot be negative.");
            }

            return await _maintenanceRepository.GetUpcomingMaintenanceAsync(daysAhead);
        }

        public async Task<IEnumerable<Maintenance>> GetOverdueMaintenanceAsync()
        {
            return await _maintenanceRepository.GetOverdueMaintenanceAsync();
        }

        public async Task<IEnumerable<Maintenance>> GetMaintenanceByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.");
            }

            return await _maintenanceRepository.GetMaintenanceByDateRangeAsync(startDate, endDate);
        }

        // Maintenance scheduling
        public async Task<Maintenance> ScheduleMaintenanceAsync(
            int vehicleId,
            MaintenanceType type,
            string description,
            decimal cost,
            DateTime scheduledDate)
        {
            // validate vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            // validate inputs
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description is required.");
            }

            if (cost < 0)
            {
                throw new ArgumentException("Cost cannot be negative.");
            }

            if (scheduledDate < DateTime.UtcNow.AddDays(-1))
            {
                throw new ArgumentException("Scheduled date cannot be in the past.");
            }

            // create maintenance record
            var maintenance = new Maintenance
            {
                VehicleId = vehicleId,
                MaintenanceType = type,
                Description = description,
                Cost = cost,
                ScheduledDate = scheduledDate,
                Status = MaintenanceStatus.Scheduled,
                CompletedDate = null
            };

            return await _maintenanceRepository.AddAsync(maintenance);
        }

        public async Task UpdateMaintenanceStatusAsync(int maintenanceId, MaintenanceStatus status)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            if (maintenance == null)
            {
                throw new KeyNotFoundException($"Maintenance with ID {maintenanceId} not found.");
            }

            // business logic for status transitions
            if (maintenance.Status == MaintenanceStatus.Completed && status != MaintenanceStatus.Completed)
            {
                throw new InvalidOperationException("Cannot change status of completed maintenance.");
            }

            // if marking as completed, update vehicle status and last service date
            if (status == MaintenanceStatus.Completed && maintenance.Status != MaintenanceStatus.Completed)
            {
                await _maintenanceRepository.UpdateMaintenanceStatusAsync(maintenanceId, status);

                // update vehicle's last service date
                await _vehicleRepository.UpdateLastServiceDateAsync(maintenance.VehicleId, DateTime.UtcNow);

                // set vehicle back to active if it was in maintenance
                var vehicle = await _vehicleRepository.GetByIdAsync(maintenance.VehicleId);
                if (vehicle?.Status == VehicleStatus.Maintenance)
                {
                    await _vehicleRepository.UpdateVehicleStatusAsync(maintenance.VehicleId, VehicleStatus.Active);
                }
            }
            else if (status == MaintenanceStatus.InProgress && maintenance.Status == MaintenanceStatus.Scheduled)
            {
                // when maintenance starts, set vehicle to maintenance status
                await _maintenanceRepository.UpdateMaintenanceStatusAsync(maintenanceId, status);
                await _vehicleRepository.UpdateVehicleStatusAsync(maintenance.VehicleId, VehicleStatus.Maintenance);
            }
            else
            {
                await _maintenanceRepository.UpdateMaintenanceStatusAsync(maintenanceId, status);
            }
        }

        public async Task CompleteMaintenanceAsync(int maintenanceId)
        {
            var maintenance = await _maintenanceRepository.GetByIdAsync(maintenanceId);
            if (maintenance == null)
            {
                throw new KeyNotFoundException($"Maintenance with ID {maintenanceId} not found.");
            }

            if (maintenance.Status == MaintenanceStatus.Completed)
            {
                throw new InvalidOperationException("Maintenance is already completed.");
            }

            // update maintenance status to completed
            await _maintenanceRepository.UpdateMaintenanceStatusAsync(maintenanceId, MaintenanceStatus.Completed);

            // update vehicle last service date
            await _vehicleRepository.UpdateLastServiceDateAsync(maintenance.VehicleId, DateTime.UtcNow);

            // set vehicle back to active if it was in maintenance
            var vehicle = await _vehicleRepository.GetByIdAsync(maintenance.VehicleId);
            if (vehicle?.Status == VehicleStatus.Maintenance)
            {
                await _vehicleRepository.UpdateVehicleStatusAsync(maintenance.VehicleId, VehicleStatus.Active);
            }
        }

        // alart and notification
        public async Task<IEnumerable<Maintenance>> GetMaintenanceAlertsAsync()
        {
            // get overdue maintenance
            var overdueMaintenance = await _maintenanceRepository.GetOverdueMaintenanceAsync();

            // get upcoming maintenance (within 7 days)
            var upcomingMaintenance = await _maintenanceRepository.GetUpcomingMaintenanceAsync(7);

            // combine both lists
            var alerts = overdueMaintenance.Concat(upcomingMaintenance)
                .OrderBy(m => m.ScheduledDate)
                .ToList();

            return alerts;
        }

        public async Task SendMaintenanceRemindersAsync()
        {
            // get maintenance due in next 3 days
            var upcomingMaintenance = await _maintenanceRepository.GetUpcomingMaintenanceAsync(3);

            // in real scenario, send notifications/emails
            foreach (var maintenance in upcomingMaintenance)
            {
                // TODO: notification logic
                // - send email to driver/admin
                // - create in-app notification
                // - log reminder sent
                Console.WriteLine($"Reminder: Maintenance {maintenance.MaintenanceId} scheduled for {maintenance.ScheduledDate:yyyy-MM-dd}");
            }

            await Task.CompletedTask;
        }

        // cost analysis
        public async Task<decimal> GetTotalMaintenanceCostByVehicleAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new KeyNotFoundException($"Vehicle with ID {vehicleId} not found.");
            }

            return await _maintenanceRepository.GetTotalMaintenanceCostByVehicleAsync(vehicleId);
        }

        public async Task<decimal> GetTotalMaintenanceCostAsync()
        {
            return await _maintenanceRepository.GetTotalMaintenanceCostAsync();
        }

        public async Task<decimal> GetMaintenanceCostByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date cannot be after end date.");
            }

            var maintenanceRecords = await _maintenanceRepository.GetMaintenanceByDateRangeAsync(startDate, endDate);

            return maintenanceRecords
                .Where(m => m.Status == MaintenanceStatus.Completed)
                .Sum(m => m.Cost);
        }
    }
}