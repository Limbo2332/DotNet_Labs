using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Extensions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;

namespace NewsSite.BLL.Services
{
    public class AuthorsService : BaseService, IAuthorsService
    {
        private readonly IAuthorsRepository _authorsRepository;

        public AuthorsService(
            UserManager<IdentityUser> userManager, 
            IMapper mapper, 
            IAuthorsRepository authorsRepository) 
            : base(userManager, mapper)
        {
            _authorsRepository = authorsRepository; 
        }

        public async Task<PageList<AuthorResponse>> GetAuthorsAsync(PageSettings? pageSettings)
        { 
            var authors = _authorsRepository.GetAll();

            if (pageSettings?.PageFiltering is not null)
            {
                var propertyValue = pageSettings.PageFiltering.PropertyValue.ToLower();

                var filteringFunc =
                    GetFilteringExpressionFunc(pageSettings.PageFiltering.PropertyName, propertyValue);

                authors = authors.Where(filteringFunc);
            }

            if (pageSettings?.PageSorting is not null)
            {
                var sortingExpression = GetSortingExpressionFunc(pageSettings.PageSorting.SortingProperty);

                authors = pageSettings.PageSorting.SortingOrder switch
                {
                    SortingOrder.Ascending => authors.OrderBy(sortingExpression),
                    SortingOrder.Descending => authors.OrderByDescending(sortingExpression),
                    _ => authors
                };
            }

            var totalItemsCount = authors.Count();

            if (pageSettings?.PagePagination is not null)
            {
                authors = authors
                    .Skip(pageSettings.PagePagination.PageSize * (pageSettings.PagePagination.PageNumber - 1))
                    .Take(pageSettings.PagePagination.PageSize);
            }

            var authorsEnumerable = await authors.ToListAsync();

            return new PageList<AuthorResponse>()
            {
                TotalCount = totalItemsCount,
                PageSize = pageSettings?.PagePagination?.PageSize ?? PageList<AuthorResponse>.MAX_PAGE_SIZE,
                PageNumber = pageSettings?.PagePagination?.PageNumber ?? 1,
                Items = _mapper.Map<List<AuthorResponse>>(authorsEnumerable)
            };
        }

        public async Task<AuthorResponse> GetAuthorByIdAsync(Guid authorId)
        {
            var author = await _authorsRepository.GetByIdAsync(authorId)
                ?? throw new Exception();

            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task<AuthorResponse> UpdateAuthorAsync(UpdatedAuthorRequest updatedAuthor)
        {
            var authorToUpdate = _mapper.Map<Author>(updatedAuthor);

            await _authorsRepository.UpdateAsync(authorToUpdate);
            await _userManager.SetEmailAsync(authorToUpdate.IdentityUser, updatedAuthor.Email);
            await _userManager.SetUserNameAsync(authorToUpdate.IdentityUser, updatedAuthor.FullName);

            return _mapper.Map<AuthorResponse>(authorToUpdate);
        }

        public async Task DeleteAuthorAsync(Guid authorId)
        {
            await _authorsRepository.DeleteAsync(authorId);
        }

        private Expression<Func<Author, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
        {
            return propertyName.ToLower() switch
            {
                "email" => author => author.Email.Contains(propertyValue),
                "fullname" => author => author.FullName.Contains(propertyValue),
                "birthdate" => author => propertyValue.ToDateTimeWithoutOutParameter(),
                "publicinformation" => author => author.PublicInformation != null
                                                 && author.PublicInformation.Contains(propertyValue),
                _ => author => true
            };
        }

        private Expression<Func<Author, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLower() switch
            {
                "email" => author => author.Email,
                "fullname" => author => author.FullName,
                "birthdate" => author => author.BirthDate,
                "publicinformation" => author => author.PublicInformation != null
                    ? author.PublicInformation.Length
                    : 0,
                _ => author => author.CreatedAt
            };
        }
    }
}
