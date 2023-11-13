using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;

namespace NewsSite.DAL.Repositories
{
    public class NewsRepository : GenericRepository<PieceOfNews>, INewsRepository
    {
        public NewsRepository(OnlineNewsContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PieceOfNews>> FilterByExpressionAsync(Expression<Func<PieceOfNews, bool>> expression)
        {
            return await _dbSet
                .Where(expression)
                .ToListAsync();
        }
    }
}
