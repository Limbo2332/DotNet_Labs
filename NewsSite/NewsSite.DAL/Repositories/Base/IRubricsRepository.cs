using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Repositories.Base
{
    public interface IRubricsRepository : IGenericRepository<Rubric>
    {
        Task<Rubric?> AddRubricForNewsIdAsync(Guid rubricId, Guid newsId);

        Task DeleteRubricForNewsIdAsync(Guid rubricId, Guid newsId);
    }
}
