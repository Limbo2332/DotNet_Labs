using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.Context;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.DAL.Repositories
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(OnlineNewsContext context) : base(context)
        {
        }

        public async Task AddNewsRubrics(Guid newsId, Guid rubricId)
        {
            await DeleteNewsRubricsByNewsIdAsync(newsId);

            var newsRubrics = new NewsRubrics
            {
                RubricId = rubricId,
                NewsId = newsId,
            };

            await _context.NewsRubrics.AddAsync(newsRubrics);
        }

        public async Task AddNewsTags(Guid newsId, List<Guid> tagsIds)
        {
            await DeleteNewsTagsByNewsIsAsync(newsId);

            var newsTags = 
                tagsIds.Select(tagId => new NewsTags
                {
                    TagId = tagId,
                    NewsId = newsId
                });

            await _context.NewsTags.AddRangeAsync(newsTags);
        }

        private async Task DeleteNewsRubricsByNewsIdAsync(Guid newsId)
        {
            var existingNewsRubrics = await _context.NewsRubrics
                .FirstOrDefaultAsync(nr => nr.NewsId == newsId);
            
            if (existingNewsRubrics is not null)
            {
                _context.NewsRubrics.Remove(existingNewsRubrics);
            }
        }

        private async Task DeleteNewsTagsByNewsIsAsync(Guid newsId)
        {
            var existingNewsTags = _context.NewsTags
                .Where(nt => nt.NewsId == newsId);

            if (await existingNewsTags.AnyAsync())
            {
                _context.NewsTags.RemoveRange(existingNewsTags);
            }
        }
    }
}
