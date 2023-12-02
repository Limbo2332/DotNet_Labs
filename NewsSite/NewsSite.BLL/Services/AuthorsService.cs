using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NewsSite.BLL.Extensions;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.DAL.Repositories.Base;
using System.Linq.Expressions;
using NewsSite.BLL.Exceptions;

namespace NewsSite.BLL.Services
{
    public class AuthorsService : BaseEntityService<Author, AuthorResponse>, IAuthorsService
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

            PageList<AuthorResponse> pageList = await base.GetAllAsync(authors, pageSettings);

            return pageList;
        }

        public async Task<AuthorResponse> GetAuthorByIdAsync(Guid authorId)
        {
            var author = await GetAuthorEntityByIdAsync(authorId);

            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task<AuthorResponse> UpdateAuthorAsync(UpdatedAuthorRequest updatedAuthor)
        {
            _ = await GetAuthorEntityByIdAsync(updatedAuthor.Id);

            var authorToUpdate = _mapper.Map<Author>(updatedAuthor);

            await _authorsRepository.UpdateAsync(authorToUpdate);
            await _userManager.SetEmailAsync(authorToUpdate.IdentityUser, updatedAuthor.Email);
            await _userManager.SetUserNameAsync(authorToUpdate.IdentityUser, updatedAuthor.FullName);

            return _mapper.Map<AuthorResponse>(authorToUpdate);
        }

        public async Task DeleteAuthorAsync(Guid authorId)
        {
            await _authorsRepository.DeleteAsync(authorId);
            await _authorsRepository.SaveChangesAsync();
        }

        public bool IsEmailUnique(string email)
        {
            return _authorsRepository.IsEmailUnique(email);
        }

        public bool IsFullNameUnique(string fullName)
        {
            return _authorsRepository.IsFullNameUnique(fullName);
        }

        public override Expression<Func<Author, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
        {
            return propertyName.ToLowerInvariant() switch
            {
                "email" => author => author.Email.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                "fullname" => author => author.FullName.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                "birthdate" => author => propertyValue.IsDateTime() 
                                         && author.BirthDate >= Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture),
                "publicinformation" => author => author.PublicInformation != null
                                                 && author.PublicInformation.ToLowerInvariant().Contains(propertyValue.ToLowerInvariant()),
                _ => author => true
            };
        }

        public override Expression<Func<Author, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLowerInvariant() switch
            {
                "email" => author => author.Email,
                "fullname" => author => author.FullName,
                "birthdate" => author => author.BirthDate,
                "publicinformation" => author => author.PublicInformation != null
                    ? author.PublicInformation
                    : 0,
                _ => author => author.UpdatedAt
            };
        }

        private async Task<Author> GetAuthorEntityByIdAsync(Guid id)
        {
            return await _authorsRepository.GetByIdAsync(id)
                   ?? throw new NotFoundException(nameof(Author), id);
        }
    }
}
