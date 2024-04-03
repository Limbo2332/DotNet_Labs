using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using System.Net;
using System.Net.Http.Json;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class NewsControllerTests : BaseControllerTests
    {
        private readonly OnlineNewsContext _dbContext;

        public NewsControllerTests(WebFactoryFixture fixture) : base(fixture)
        {
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task GetNewsByRubric_ShouldReturnOk()
        {
            // Arrange
            var rubricId = _dbContext.NewsRubrics.First().RubricId;

            var query = new Dictionary<string, string?>()
            {
                ["rubricId"] = rubricId.ToString(),
                ["PageSorting.SortingProperty"] = nameof(News.Content),
                ["PageSorting.SortingOrder"] = SortingOrder.Ascending.ToString()
            };

            var requestUri = QueryHelpers.AddQueryString("api/news/by-rubric", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent =
                await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInAscendingOrder(i => i.Content);
        }

        [Fact]
        public async Task GetNewsByTags_ShouldReturnOk()
        {
            // Arrange
            var tagsIds = _dbContext.NewsTags.First().TagId;

            var query = new Dictionary<string, string>()
            {
                ["tagsIds"] = tagsIds.ToString(),
                ["PageSorting.SortingProperty"] = nameof(News.Subject),
                ["PageSorting.SortingOrder"] = SortingOrder.Descending.ToString()
            };

            var requestUri = QueryHelpers.AddQueryString("api/news/by-tags", query!);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent =
                await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInDescendingOrder(i => i.Subject);
        }
    }
}
