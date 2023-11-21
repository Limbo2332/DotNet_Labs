using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Repositories.Base
{
    public interface INewsRepository : IGenericRepository<News>
    {
        Task AddNewsRubrics(Guid newsId, List<Guid> rubricId);

        Task AddNewsTags(Guid newsId, List<Guid> tagsIds);
    }
}
