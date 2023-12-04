using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.Entities;
using NewsSite.UnitTests.TestData;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class AuthorsControllerTests : BaseControllerTests
    {
        private readonly OnlineNewsContext _dbContext;

        public AuthorsControllerTests(WebFactoryFixture fixture) : base(fixture)
        {
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task GetAuthors_ShouldReturnOk()
        {
            // Arrange
            var query = new Dictionary<string, string>()
            {
                ["PageSorting.SortingProperty"] = nameof(Author.FullName),
                ["PageSorting.SortingOrder"] = SortingOrder.Descending.ToString()
            };

            var requestUri = QueryHelpers.AddQueryString("api/authors", query!);

            // Act
            var response = await _httpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = 
                await response.Content.ReadFromJsonAsync<PageList<AuthorResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInDescendingOrder(i => i.FullName);
        }

        [Fact]
        public async Task GetAuthorById_ShouldReturnOk()
        {
            // Arrange
            var author = _dbContext.Authors.First();

            // Act
            var response = await _httpClient.GetAsync($"api/authors/{author.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent =
                await response.Content.ReadFromJsonAsync<AuthorResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(author.Id);
        }

        [Fact]
        public async Task DeleteAuthor_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();
            var author = _dbContext.Authors.Last();

            // Act
            var response = await _httpClient.DeleteAsync($"api/authors/{author.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
