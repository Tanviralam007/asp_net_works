using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFMS.DAL.Entities;

namespace STFMS.BLL.Interfaces.Services
{
    public interface IDriverAssignmentService
    {
        // automatic driver assignment signatures
        Task<Driver?> AssignDriverToBookingAsync(int bookingId);
        Task<Driver?> FindBestAvailableDriverAsync(string pickupLocation);
        Task<IEnumerable<Driver>> FindNearbyDriversAsync(string location, int maxDistance);

        // assignment logic signatures
        Task<Driver?> GetHighestRatedAvailableDriverAsync();
        Task<bool> IsDriverAvailableForBookingAsync(int driverId, DateTime bookingTime);

        // vehicle assignment signatures
        Task<Vehicle?> AssignVehicleToBookingAsync(int driverId, ServiceType serviceType);
        Task<Vehicle?> GetAvailableVehicleForDriverAsync(int driverId);

        // optimization signatures
        Task<int> GetDriverWorkloadAsync(int driverId);
        Task BalanceDriverWorkloadAsync();
    }
}
