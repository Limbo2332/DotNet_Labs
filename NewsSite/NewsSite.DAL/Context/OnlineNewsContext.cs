using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context.EntityConfigurations;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Entities.Abstract;
using System.Reflection.Emit;

namespace NewsSite.DAL.Context
{
    public class OnlineNewsContext : IdentityDbContext
    {
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<PieceOfNews> News => Set<PieceOfNews>();
        public DbSet<Rubric> Rubrics => Set<Rubric>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<NewsRubrics> NewsRubrics => Set<NewsRubrics>();
        public DbSet<NewsTags> NewsTags => Set<NewsTags>();

        public OnlineNewsContext(DbContextOptions<OnlineNewsContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AuthorConfig).Assembly);

            base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var newEntries = ChangeTracker.Entries()
                .Where(entryEntity => entryEntity.State == EntityState.Added
                    && entryEntity.Entity != null
                    && entryEntity.Entity as BaseEntity != null)
                .Select(entryEntity => entryEntity.Entity as BaseEntity);

            foreach (var newEntry in newEntries)
            {
                newEntry!.CreatedAt = DateTime.Now;
                newEntry!.UpdatedAt = DateTime.Now;
            }

            var updatedEntries = ChangeTracker.Entries()
                .Where(entryEntity => entryEntity.State == EntityState.Modified
                    && entryEntity.Entity != null
                    && entryEntity.Entity as BaseEntity != null)
                .Select(entryEntity => entryEntity.Entity as BaseEntity);

            foreach (var updatedEntry in updatedEntries)
            {
                updatedEntry!.UpdatedAt = DateTime.Now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
