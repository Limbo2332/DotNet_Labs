using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;

namespace NewsSite.BLL.Services
{
    public class TagsService : BaseEntityService<Tag, TagResponse>, ITagsService
    {
        private readonly ITagsRepository _tagsRepository;

        public TagsService(
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            ITagsRepository tagsRepository)
            : base(userManager, mapper)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task<PageList<TagResponse>> GetAllTagsAsync(PageSettings? pageSettings)
        {
            var tags = _tagsRepository.GetAll();

            PageList<TagResponse> pageList = await base.GetAllAsync(tags, pageSettings);

            return pageList;
        }

        public async Task<TagResponse> GetTagByIdAsync(Guid id)
        {
            var tag = await _tagsRepository.GetByIdAsync(id)
                ?? throw new Exception();

            return _mapper.Map<TagResponse>(tag);
        }

        public async Task<TagResponse> AddTagForNewsIdAsync(Guid tagId, Guid newsId)
        {
            var tag = await _tagsRepository.AddTagForNewsIdAsync(tagId, newsId)
                ?? throw new Exception();

            return _mapper.Map<TagResponse>(tag);
        }

        public async Task<TagResponse> CreateNewTagAsync(NewTagRequest newTag)
        {
            var tag = _mapper.Map<Tag>(newTag);

            await _tagsRepository.AddAsync(tag);
            await _tagsRepository.SaveChangesAsync();

            return _mapper.Map<TagResponse>(tag);
        }

        public async Task<TagResponse> UpdateTagAsync(UpdateTagRequest newTag)
        {
            var tag = _mapper.Map<Tag>(newTag);

            await _tagsRepository.UpdateAsync(tag);

            return _mapper.Map<TagResponse>(tag);
        }

        public async Task DeleteTagAsync(Guid id)
        {
            await _tagsRepository.DeleteAsync(id);
            await _tagsRepository.SaveChangesAsync();
        }

        public async Task DeleteTagForNewsIdAsync(Guid tagId, Guid newsId)
        {
            await _tagsRepository.DeleteTagForNewsIdAsync(tagId, newsId);
        }

        public override Expression<Func<Tag, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
        {
            return propertyName.ToLower() switch
            {
                "name" => tag => tag.Name.Contains(propertyValue),
                _ => tag => true
            };
        }

        public override Expression<Func<Tag, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLower() switch
            {
                "name" => tag => tag.Name.Length,
                _ => tag => tag.UpdatedAt
            };
        }
    }
}
