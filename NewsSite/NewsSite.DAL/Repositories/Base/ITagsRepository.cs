using NewsSite.DAL.Entities;

namespace NewsSite.DAL.Repositories.Base
{
    public interface ITagsRepository : IGenericRepository<Tag>
    {
        Task<Tag?> AddTagForNewsIdAsync(Guid tagId, Guid newsId);

        Task DeleteTagForNewsIdAsync(Guid tagId, Guid newsId);
    }
}
