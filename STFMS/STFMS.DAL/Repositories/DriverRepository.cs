using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STFMS.DAL.Repositories
{
    public class DriverRepository : GenericRepository<Driver>, IDriverRepository
    {
        public DriverRepository(AppDbContext context) 
            : base(context)
        {

        }

        public override async Task<Driver?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(d => d.User)  
                .FirstOrDefaultAsync(d => d.DriverId == id);
        }

        public override async Task<IEnumerable<Driver>> GetAllAsync()
        {
            return await _dbSet
                .Include(d => d.User)  
                .OrderBy(d => d.User.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetAvailableDriversAsync()
        {
            return await _dbSet
                .Include(u => u.User)
                .Where(d => d.Status == DriverStatus.Available)
                .OrderByDescending(r => r.Rating)
                .ToListAsync();
        }

        public async Task<Driver?> GetDriverByLicenseNumberAsync(string licenseNumber)
        {
            return await _dbSet
                .Include(u => u.User)
                .FirstOrDefaultAsync(d => d.LicenseNumber == licenseNumber);
        }

        public async Task<Driver?> GetDriverByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(u => u.User)
                .FirstOrDefaultAsync(d => d.UserId == userId);
        }

        public async Task<IEnumerable<Driver>> GetDriversByStatusAsync(DriverStatus status)
        {
            return await _dbSet
                .Include(u => u.User)
                .Where(_dbSet => _dbSet.Status == status)
                .OrderBy(d => d.User.FullName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Driver>> GetDriversWithLowRatingAsync(decimal ratingThreshold)
        {
            return await _dbSet
                .Include(u => u.User)
                .Where(d => d.Rating < ratingThreshold && d.TotalRides > 0)
                .OrderBy(d => d.Rating)
                .ToListAsync();
        }

        public async Task<Driver?> GetDriverWithBookingsAsync(int driverId)
        {
            return await _dbSet
                .Include(u => u.User)
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Feedback)
                .FirstOrDefaultAsync(d => d.DriverId == driverId);
        }

        public async Task<Driver?> GetDriverWithVehiclesAsync(int driverId)
        {
            return await _dbSet
                .Include (u => u.User)
                .Include(u => u.Vehicles)
                .FirstOrDefaultAsync(d => d.DriverId == driverId);
        }

        public async Task<IEnumerable<Driver>> GetTopRatedDriversAsync(int count)
        {
            return await _dbSet
                .Include(u => u.User)
                .OrderByDescending(d => d.Rating)
                .ThenByDescending(d => d.TotalRides)
                .Take(count)
                .ToListAsync();
        }

        public async Task IncrementTotalRidesAsync(int driverId)
        {
            var driver = await _dbSet.FindAsync(driverId);
            if(driver != null)
            {
                driver.TotalRides++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDriverRatingAsync(int driverId, decimal newRating)
        {
            var driver = await _dbSet.FindAsync(driverId);
            if(driver != null)
            {
                driver.Rating = newRating;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDriverStatusAsync(int driverId, DriverStatus status)
        {
            var driver = await _dbSet.FindAsync(driverId);
            if(driver != null)
            {
                driver.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
