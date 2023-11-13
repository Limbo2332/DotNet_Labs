using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories
{
    public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository
    {
        public AuthorsRepository(OnlineNewsContext context) : base(context)
        {
        }

        public async Task<Author> GetAuthorByEmailAsync(string email)
        {
            return await _dbSet.FirstAsync(a => a.Email == email);
        }
    }
}
