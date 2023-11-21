using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;

namespace NewsSite.DAL.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(OnlineNewsContext context) : base(context)
        {
        }
    }
}
