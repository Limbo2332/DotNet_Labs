using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;

namespace NewsSite.BLL.Services
{
    public class NewsService : BaseEntityService<News, NewsResponse>, INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IAuthorsRepository _authorsRepository;

        public NewsService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            INewsRepository newsRepository,
            IAuthorsRepository authorsRepository)
            : base(userManager, mapper)
        {
            _newsRepository = newsRepository;
            _authorsRepository = authorsRepository;
        }

        public async Task<PageList<NewsResponse>> GetNewsAsync(PageSettings? pageSettings)
        {
            var news = _newsRepository.GetAll();

            PageList<NewsResponse> pageList = await base.GetAllAsync(news, pageSettings);

            return pageList;
        }

        public async Task<PageList<NewsResponse>> GetNewsByRubricAsync(Guid rubricId, PageSettings? pageSettings)
        {
            var news =
                _newsRepository.GetAll()
                    .Include(n => n.NewsRubrics)
                    .Where(n => n.NewsRubrics!.Any(nr => nr.RubricId == rubricId));

            PageList<NewsResponse> pageList = await base.GetAllAsync(news, pageSettings);

            return pageList;
        }

        public async Task<PageList<NewsResponse>> GetNewsByTagsAsync(List<Guid> tagsIds, PageSettings? pageSettings)
        {
            var news =
                _newsRepository.GetAll()
                    .Include(n => n.NewsTags)
                    .Where(n => n.NewsTags!.Any(nt => tagsIds.Contains(nt.TagId)));

            PageList<NewsResponse> pageList = await base.GetAllAsync(news, pageSettings);

            return pageList;
        }

        public async Task<PageList<NewsResponse>> GetNewsByAuthorAsync(Guid authorId, PageSettings? pageSettings)
        {
            var news =
                _newsRepository.GetAll()
                    .Where(n => n.CreatedBy == authorId);

            PageList<NewsResponse> pageList = await base.GetAllAsync(news, pageSettings);

            return pageList;
        }

        public async Task<PageList<NewsResponse>> GetNewsByPeriodOfTimeAsync(DateTime startDate, DateTime endDate, PageSettings? pageSettings)
        {
            var news =
                _newsRepository.GetAll()
                    .Where(n => n.UpdatedAt >= startDate && n.UpdatedAt <= endDate);

            PageList<NewsResponse> pageList = await base.GetAllAsync(news, pageSettings);

            return pageList;
        }

        public async Task<NewsResponse> GetNewsByIdAsync(Guid id)
        {
            var news = await _newsRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(nameof(News), id);

            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> CreateNewNewsAsync(NewNewsRequest newNewsRequest)
        {
            var newNews = _mapper.Map<News>(newNewsRequest);

            _ = await _authorsRepository.GetByIdAsync(newNewsRequest.AuthorId)
                ?? throw new NotFoundException(nameof(Author), newNewsRequest.AuthorId);

            await _newsRepository.AddAsync(newNews);

            if (newNewsRequest.RubricsIds is not null && newNewsRequest.RubricsIds.Any())
            {
                await _newsRepository.AddNewsRubricsAsync(newNews.Id, newNewsRequest.RubricsIds);
            }

            if (newNewsRequest.TagsIds is not null && newNewsRequest.TagsIds.Any())
            {
                await _newsRepository.AddNewsTagsAsync(newNews.Id, newNewsRequest.TagsIds);
            }

            await _newsRepository.SaveChangesAsync();

            return _mapper.Map<NewsResponse>(newNews);
        }

        public async Task<NewsResponse> UpdateNewsAsync(UpdateNewsRequest updateNewsRequest)
        {
            var news = await GetNewsByIdAsync(updateNewsRequest.Id);
            var updateNews = _mapper.Map<News>(updateNewsRequest);

            updateNews.CreatedBy = news.AuthorId;

            await _newsRepository.UpdateAsync(updateNews);
            await _newsRepository.SaveChangesAsync();

            return _mapper.Map<NewsResponse>(updateNews);
        }

        public async Task DeleteNewsAsync(Guid newsId)
        {
            await _newsRepository.DeleteAsync(newsId);
            await _newsRepository.SaveChangesAsync();
        }

        public override Expression<Func<News, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
        {
            return propertyName.ToLowerInvariant() switch
            {
                "content" => news => news.Content.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                "subject" => news => news.Subject.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                _ => news => true
            };
        }

        public override Expression<Func<News, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLowerInvariant() switch
            {
                "content" => news => news.Content,
                "subject" => news => news.Subject,
                _ => news => news.UpdatedAt
            };
        }
    }
}
