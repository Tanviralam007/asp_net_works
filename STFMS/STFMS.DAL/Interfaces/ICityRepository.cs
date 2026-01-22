using STFMS.DAL.Entities;

namespace STFMS.DAL.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<City?> GetCityByNameAsync(string cityName);
        Task<IEnumerable<City>> GetActiveCitiesAsync();
        Task<IEnumerable<City>> GetCitiesByFareRangeAsync(decimal minBaseFare, decimal maxBaseFare);
    }
}
