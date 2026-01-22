using Microsoft.EntityFrameworkCore;
using STFMS.DAL.Data;
using STFMS.DAL.Entities;
using STFMS.DAL.Interfaces;

namespace STFMS.DAL.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) 
            : base(context)
        {

        }

        public async Task<City?> GetCityByNameAsync(string cityName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.CityName == cityName);
        }

        public async Task<IEnumerable<City>> GetActiveCitiesAsync()
        {
            return await _dbSet
                .Where(c => c.IsActive)
                .OrderBy(c => c.CityName)
                .ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesByFareRangeAsync(decimal minBaseFare, decimal maxBaseFare)
        {
            return await _dbSet
                .Where(c => c.BaseFare >= minBaseFare && c.BaseFare <= maxBaseFare)
                .OrderBy(c => c.BaseFare)
                .ToListAsync();
        }
    }
}
