﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories
{
    public class AuthorsRepository : GenericRepository<Author>, IAuthorsRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthorsRepository(
            OnlineNewsContext context,
            UserManager<IdentityUser> userManager)
            : base(context)
        {
            _userManager = userManager;
        }

        public async Task<Author?> GetAuthorByEmailAsync(string email)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email);
        }

        public bool IsEmailUnique(string email)
        {
            return !_dbSet.Any(a => a.Email == email);
        }

        public bool IsFullNameUnique(string fullName)
        {
            return !_dbSet.Any(a => a.FullName == fullName);
        }

        public override async Task DeleteAsync(Guid id)
        {
            var author = await GetByIdAsync(id);

            if (author is not null)
            {
                var identityUser = await _userManager.FindByEmailAsync(author.Email);
                if (identityUser is not null)
                {
                    await _userManager.DeleteAsync(identityUser);
                }

                _dbSet.Remove(author);
                await _context.SaveChangesAsync();
            }
        }
    }
}
