using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Repositories.Base
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(Guid id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task<int> SaveChangesAsync();
    }
}
