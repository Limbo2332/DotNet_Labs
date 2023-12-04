using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context.EntityConfigurations;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Entities.Abstract;

namespace NewsSite.DAL.Context
{
    public class OnlineNewsContext : IdentityDbContext
    {
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<News> News => Set<News>();
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
            BaseSaveChanges();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void BaseSaveChanges()
        {
            var newEntries = ChangeTracker.Entries()
                .Where(entryEntity => entryEntity is { State: EntityState.Added, Entity: BaseEntity })
                .Select(entryEntity => entryEntity.Entity as BaseEntity);

            foreach (var newEntry in newEntries)
            {
                newEntry!.CreatedAt = DateTime.UtcNow;
                newEntry.UpdatedAt = DateTime.UtcNow;
            }

            var updatedEntries = ChangeTracker.Entries()
                .Where(entryEntity => entryEntity is { State: EntityState.Modified, Entity: BaseEntity })
                .Select(entryEntity => entryEntity.Entity as BaseEntity);

            foreach (var updatedEntry in updatedEntries)
            {
                updatedEntry!.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
