using System.Linq.Expressions;

namespace STFMS.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // basic crud operations jegula onno jaygay easily envoke kora jabe
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        // additional kicho methods 
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task SaveChangesAsync();
    }
}
