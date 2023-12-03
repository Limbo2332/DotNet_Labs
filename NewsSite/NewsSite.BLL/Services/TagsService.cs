using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;
using NewsSite.BLL.Exceptions;

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
                ?? throw new NotFoundException(nameof(Tag), id);

            return _mapper.Map<TagResponse>(tag);
        }

        public async Task<TagResponse> AddTagForNewsIdAsync(Guid tagId, Guid newsId)
        {
            var newsTag = await _tagsRepository.AddTagForNewsIdAsync(tagId, newsId)
                ?? throw new NotFoundException(nameof(Tag), tagId, nameof(News), newsId);

            var tag = await GetTagByIdAsync(newsTag.TagId);

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
            _ = await GetTagByIdAsync(newTag.Id);

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
            return propertyName.ToLowerInvariant() switch
            {
                "name" => tag => tag.Name.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                _ => tag => true
            };
        }

        public override Expression<Func<Tag, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLowerInvariant() switch
            {
                "name" => tag => tag.Name,
                _ => tag => tag.UpdatedAt
            };
        }
    }
}
