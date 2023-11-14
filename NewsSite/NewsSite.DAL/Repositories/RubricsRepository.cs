using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories
{
    public class RubricsRepository : GenericRepository<Rubric>, IRubricsRepository
    {
        public RubricsRepository(OnlineNewsContext context) : base(context)
        {
        }
    }
}
