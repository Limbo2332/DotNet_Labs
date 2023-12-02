using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;
using NewsSite.BLL.Exceptions;

namespace NewsSite.BLL.Services
{
    public class RubricsService : BaseEntityService<Rubric, RubricResponse>, IRubricsService
    {
        private readonly IRubricsRepository _rubricsRepository;

        public RubricsService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IRubricsRepository rubricsRepository)
            : base(userManager, mapper)
        {
            _rubricsRepository = rubricsRepository;
        }

        public async Task<PageList<RubricResponse>> GetAllRubricsAsync(PageSettings? pageSettings)
        {
            var rubrics = _rubricsRepository.GetAll();

            PageList<RubricResponse> pageList = await base.GetAllAsync(rubrics, pageSettings);

            return pageList;
        }

        public async Task<RubricResponse> GetRubricByIdAsync(Guid id)
        {
            var rubric = await _rubricsRepository.GetByIdAsync(id) 
                         ?? throw new NotFoundException(nameof(Rubric), id);

            return _mapper.Map<RubricResponse>(rubric);
        }

        public async Task<RubricResponse> AddRubricForNewsIdAsync(Guid rubricId, Guid newsId)
        {
            var rubric = await _rubricsRepository.AddRubricForNewsIdAsync(rubricId, newsId)
                      ?? throw new NotFoundException(nameof(Rubric), rubricId);

            return _mapper.Map<RubricResponse>(rubric);
        }

        public async Task<RubricResponse> CreateNewRubricAsync(NewRubricRequest newRubric)
        {
            var rubric = _mapper.Map<Rubric>(newRubric);

            await _rubricsRepository.AddAsync(rubric);
            await _rubricsRepository.SaveChangesAsync();

            return _mapper.Map<RubricResponse>(rubric);
        }

        public async Task<RubricResponse> UpdateRubricAsync(UpdateRubricRequest newRubric)
        {
            var rubric = _mapper.Map<Rubric>(newRubric);

            await _rubricsRepository.UpdateAsync(rubric);

            return _mapper.Map<RubricResponse>(rubric);
        }

        public async Task DeleteRubricAsync(Guid id)
        {
            await _rubricsRepository.DeleteAsync(id);
            await _rubricsRepository.SaveChangesAsync();
        }

        public async Task DeleteRubricForNewsIdAsync(Guid rubricId, Guid newsId)
        {
            await _rubricsRepository.DeleteRubricForNewsIdAsync(rubricId, newsId);
        }

        public override Expression<Func<Rubric, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
        {
            return propertyName.ToLowerInvariant() switch
            {
                "name" => rubric => rubric.Name.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                _ => rubric => true
            };
        }

        public override Expression<Func<Rubric, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLowerInvariant() switch
            {
                "name" => rubric => rubric.Name,
                _ => rubric => rubric.UpdatedAt
            };
        }
    }
}
