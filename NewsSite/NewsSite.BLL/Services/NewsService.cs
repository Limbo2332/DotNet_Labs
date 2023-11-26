using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;
using NewsSite.BLL.Exceptions;

namespace NewsSite.BLL.Services
{
    public class NewsService : BaseEntityService<News, NewsResponse>, INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            INewsRepository newsRepository)
            : base(userManager, mapper)
        {
            _newsRepository = newsRepository;
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

        public async Task<PageList<NewsResponse>> GetNewsByTagsAsync(List<Guid> tagsId, PageSettings? pageSettings)
        {
            var news =
                _newsRepository.GetAll()
                    .Include(n => n.NewsTags)
                    .Where(n => n.NewsTags!.Any(nt => tagsId.Contains(nt.TagId)));

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
                ?? throw new NotFoundException(nameof(News));

            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> CreateNewNewsAsync(NewNewsRequest newNewsRequest)
        {
            var newNews = _mapper.Map<News>(newNewsRequest);

            await _newsRepository.AddAsync(newNews);

            if (newNewsRequest.RubricsIds is not null && newNewsRequest.RubricsIds.Any())
            {
                await _newsRepository.AddNewsRubrics(newNews.Id, newNewsRequest.RubricsIds);
            }

            if (newNewsRequest.TagsIds is not null && newNewsRequest.TagsIds.Any())
            {
                await _newsRepository.AddNewsTags(newNews.Id, newNewsRequest.TagsIds);
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
            return propertyName.ToLower() switch
            {
                "content" => news => news.Content.Contains(propertyValue),
                "subject" => news => news.Subject.Contains(propertyValue),
                _ => news => true
            };
        }

        public override Expression<Func<News, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLower() switch
            {
                "content" => news => news.Content.Length,
                "subject" => news => news.Subject,
                _ => news => news.UpdatedAt
            };
        }
    }
}
