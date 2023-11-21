using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.Repositories.Base;

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
                    .Include(n => n.NewsRubrics!.Where(nr => nr.RubricId == rubricId))
                        .ThenInclude(nr => nr.Rubric)
                    .Where(n => n.NewsRubrics!.Any());

            PageList<NewsResponse> pageList = await base.GetAllAsync(news, pageSettings);

            return pageList;
        }

        public async Task<PageList<NewsResponse>> GetNewsByTagsAsync(List<Guid> tagsId, PageSettings? pageSettings)
        {
            var news =
                _newsRepository.GetAll()
                    .Include(n => n.NewsTags!.Where(nt => tagsId.Contains(nt.TagId)))
                        .ThenInclude(nt => nt.Tag)
                    .Where(n => n.NewsRubrics!.Any());

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
                ?? throw new Exception();

            return _mapper.Map<NewsResponse>(news);
        }

        public async Task<NewsResponse> CreateNewNewsAsync(NewNewsRequest newNewsRequest)
        {
            var newNews = _mapper.Map<News>(newNewsRequest);
            await _newsRepository.AddAsync(newNews);

            return _mapper.Map<NewsResponse>(newNews);
        }

        public async Task<NewsResponse> UpdateNewsAsync(UpdateNewsRequest updateNewsRequest)
        {
            var news = await GetNewsByIdAsync(updateNewsRequest.Id);
            var updateNews = _mapper.Map<News>(updateNewsRequest);

            updateNews.CreatedBy = news.AuthorId;

            await _newsRepository.UpdateAsync(updateNews);

            return _mapper.Map<NewsResponse>(updateNews);
        }

        public async Task DeleteNewsAsync(Guid newsId)
        {
            await _newsRepository.DeleteAsync(newsId);
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
