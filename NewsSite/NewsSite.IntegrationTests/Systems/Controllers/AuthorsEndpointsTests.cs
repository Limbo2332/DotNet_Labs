using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using System.Net.Http.Json;
using System.Net;
using Bogus;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.Entities;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class AuthorsEndpointsTests(WebFactoryFixture fixture) : BaseControllerTests(fixture)
    {
        private readonly OnlineNewsContext _dbContext = fixture.DbContext;

        #region GetAuthorsAsync Tests

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnOk()
        {
            // Arrange
            var query = new Dictionary<string, string?>
            {
                ["PageSorting.SortingProperty"] = "FullName",
                ["PageSorting.SortingOrder"] = SortingOrder.Descending.ToString()
            };

            var requestUri = QueryHelpers.AddQueryString("api/authors", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<AuthorResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInDescendingOrder(i => i.FullName);
        }

        #endregion

        #region GetAuthorById Tests

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnOk_WhenAuthorExists()
        {
            // Arrange
            var authorId = _dbContext.Authors.First().Id;

            // Act
            var response = await HttpClient.GetAsync($"api/authors/{authorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<AuthorResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(authorId);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldReturnNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = Guid.Empty;
            var exceptionMessage = new NotFoundException(nameof(Author), authorId).Message;
            
            // Act
            var response = await HttpClient.GetAsync($"api/authors/{authorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Message.Should().Be(exceptionMessage);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region UpdateAuthorAsync Tests

        [Fact]
        public async Task UpdateAuthorAsync_ShouldReturnOk_WhenValidationIsPassed()
        {
            // Arrange
            await AuthenticateAsync();
            
            var author = _dbContext.Authors.AsNoTracking().First();
            var updatedAuthorRequest = new UpdatedAuthorRequest
            {
                Id = author.Id,
                Email = "testemail@gmail.com",
                FullName = "FullNameFullName",
                BirthDate = new DateTime(1990, 11, 1)
            };

            // Act
            var response = await HttpClient.PutAsJsonAsync("api/authors", updatedAuthorRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<AuthorResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().NotBe(Guid.Empty);
            responseContent.Email.Should().Be(updatedAuthorRequest.Email);
            responseContent.FullName.Should().Be(updatedAuthorRequest.FullName);
            responseContent.BirthDate.Should().Be(updatedAuthorRequest.BirthDate);
            responseContent.Sex.Should().Be(null);
            responseContent.PublicInformation.Should().Be(null);
        }

        [Fact]
        public async Task UpdateAuthorAsync_ShouldReturnBadRequest_WhenValidationNotPassed()
        {
            // Arrange
            await AuthenticateAsync();
            
            var author = _dbContext.Authors.AsNoTracking().First();
            var updatedAuthorRequest = new UpdatedAuthorRequest
            {
                Id = author.Id,
                Email = string.Empty,
                FullName = string.Empty,
                BirthDate = DateTime.Today.AddYears(1)
            };
            
            // Act
            var response = await HttpClient.PutAsJsonAsync("api/authors", updatedAuthorRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Message.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region DeleteAuthorAsync Tests

        [Fact]
        public async Task DeleteAuthorAsync_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();
            var author = _dbContext.Authors.First();

            // Act
            var response = await HttpClient.DeleteAsync($"api/authors/{author.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            
            // Cleanup
            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();
        }

        #endregion

    }
}
