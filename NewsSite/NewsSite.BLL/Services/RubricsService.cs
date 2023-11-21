using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

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
                ?? throw new Exception();

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

        public override Expression<Func<Rubric, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
        {
            return propertyName.ToLower() switch
            {
                "name" => news => news.Name.Contains(propertyValue),
                _ => news => true
            };
        }

        public override Expression<Func<Rubric, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLower() switch
            {
                "name" => news => news.Name.Length,
                _ => news => news.UpdatedAt
            };
        }
    }
}
