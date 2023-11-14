using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories
{
    public class TagsRepository : GenericRepository<Tag>, ITagsRepository
    {
        public TagsRepository(OnlineNewsContext context) : base(context)
        {
        }
    }
}
