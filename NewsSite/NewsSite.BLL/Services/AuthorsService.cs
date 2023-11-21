using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public override Expression<Func<Author, bool>> GetFilteringExpressionFunc(string propertyName, string propertyValue)
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

        public override Expression<Func<Author, object>> GetSortingExpressionFunc(string sortingValue)
        {
            return sortingValue.ToLower() switch
            {
                "email" => author => author.Email,
                "fullname" => author => author.FullName,
                "birthdate" => author => author.BirthDate,
                "publicinformation" => author => author.PublicInformation != null
                    ? author.PublicInformation.Length
                    : 0,
                _ => author => author.UpdatedAt
            };
        }
    }
}
