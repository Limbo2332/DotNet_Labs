using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.WebUtilities;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using System.Net;
using System.Net.Http.Json;
using Bogus;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Request.News;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class NewsControllerTests(WebFactoryFixture fixture) : BaseControllerTests(fixture)
    {
        private readonly OnlineNewsContext _dbContext = fixture.DbContext;

        #region GetNewsAsync Tests

        [Fact]
        public async Task GetNewsAsync_ShouldReturnOkResult()
        {
            // Act
            var response = await HttpClient.GetAsync("api/news");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
        }

        [Fact]
        public async Task GetNewsAsync_ShouldBeSorted_WhenPageSortingIsApplied()
        {
            // Arrange
            var query = new Dictionary<string, string?>
            {
                ["PageSorting.SortingProperty"] = "Subject",
                ["PageSorting.SortingOrder"] = "desc"
            };

            var requestUri = QueryHelpers.AddQueryString("api/news", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInDescendingOrder(i => i.Subject);
        }

        [Fact]
        public async Task GetNewsAsync_ShouldBeFiltered_WhenPageFilteringIsApplied()
        {
            // Arrange
            var query = new Dictionary<string, string?>
            {
                ["PageFiltering.PropertyName"] = "Subject",
                ["PageFiltering.PropertyValue"] = "u"
            };

            var requestUri = QueryHelpers.AddQueryString("api/news", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().Contain(x => x.Subject.Contains("u"));
        }

        [Fact]
        public async Task GetNewsAsync_ShouldBePaginated_WhenPagePaginationIsApplied()
        {
            // Arrange
            var query = new Dictionary<string, string?>
            {
                ["PagePagination.PageSize"] = "2",
                ["PagePagination.PageNumber"] = "3"
            };

            var requestUri = QueryHelpers.AddQueryString("api/news", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Count.Should().Be(1);
        }

        #endregion

        #region GetNewsByRubricAsync Tests

        [Fact]
        public async Task GetNewsByRubricAsync_ShouldReturnOk()
        {
            // Arrange
            var rubricId = _dbContext.NewsRubrics.First().RubricId;

            var query = new Dictionary<string, string?>()
            {
                ["rubricId"] = rubricId.ToString(),
                ["PageSorting.SortingProperty"] = nameof(News.Content),
                ["PageSorting.SortingOrder"] = SortingOrder.Ascending.ToString()
            };

            var requestUri = QueryHelpers.AddQueryString($"api/news/by-rubric/{rubricId}", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent =
                await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInAscendingOrder(i => i.Content);
        }

        #endregion

        #region GetNewsByTagsAsync Tests

        [Fact]
        public async Task GetNewsByTagsAsync_ShouldReturnOk()
        {
            // Arrange
            var tagsIds = _dbContext.NewsTags.Take(2).Select(t => t.TagId).ToList();

            var query = new Dictionary<string, string?>()
            {
                ["TagsIds"] = tagsIds.ToString(),
                ["PageSorting.SortingProperty"] = nameof(News.Subject),
                ["PageSorting.SortingOrder"] = SortingOrder.Descending.ToString()
            };

            var requestUri = QueryHelpers.AddQueryString("api/news/by-tags", query);

            // Act
            var response = await HttpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Should().BeInDescendingOrder(i => i.Subject);
        }

        #endregion

        #region GetNewsByAuthorAsync Tests

        [Fact]
        public async Task GetNewsByAuthorAsync_ShouldReturnOk()
        {
            // Arrange
            var news = _dbContext.News.AsNoTracking().First();
            var authorId = news.CreatedBy;
            
            // Act
            var response = await HttpClient.GetAsync($"api/news/by-author/{authorId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Select(x => x.AuthorId).Should().Contain(authorId);
        }

        #endregion

        #region GetNewsByIdAsync Tests

        [Fact]
        public async Task GetNewsByIdAsync_ShouldReturnOk()
        {
            // Arrange
            var newsId = _dbContext.News.AsNoTracking().First().Id;
            
            // Act
            var response = await HttpClient.GetAsync($"api/news/{newsId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<NewsResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(newsId);
        }

        #endregion
        
        #region GetNewsByIdAsync Tests

        [Fact]
        public async Task GetNewsByPeriodOfTimeAsync_ShouldReturnOk()
        {
            // Arrange
            var news = await _dbContext.News.AsNoTracking().FirstAsync();
            var startDate = news.UpdatedAt.ToString("yyyy-MM-dd");
            var endDate = new DateTime(news.UpdatedAt.Year + 1, news.UpdatedAt.Month, news.UpdatedAt.Day).ToString("yyyy-MM-dd");
            
            // Act
            var response = await HttpClient.GetAsync($"api/news/by-date/{startDate}/{endDate}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<PageList<NewsResponse>>();

            responseContent.Should().NotBeNull();
            responseContent!.Items.Select(x => x.Id).Should().Contain(news.Id);
        }

        #endregion

        #region CreateNewsAsync Tests

        [Fact]
        public async Task CreateNewsAsync_ShouldReturnCreated_WhenValidationIsPassed()
        {
            // Arrange
            await AuthenticateAsync();

            var authorId = _dbContext.Authors.AsNoTracking().First().Id;
            var newNewsRequest = new NewNewsRequest
            {
                Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1),
                Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH - 1),
                AuthorId = authorId
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync("api/news", newNewsRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadFromJsonAsync<NewsResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().NotBe(Guid.Empty);
            responseContent.AuthorId.Should().Be(newNewsRequest.AuthorId);
            responseContent.Content.Should().Be(newNewsRequest.Content);
            responseContent.Subject.Should().Be(newNewsRequest.Subject);

            // Cleanup
            var newsToRemove = _dbContext.News.First(t => t.Id == responseContent.Id);
            _dbContext.News.Remove(newsToRemove);
            await _dbContext.SaveChangesAsync();
        }
        
        [Fact]
        public async Task CreateNewsAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
        {
            // Arrange
            await AuthenticateAsync();

            var authorId = _dbContext.Authors.AsNoTracking().First().Id;
            var newNewsRequest = new NewNewsRequest
            {
                Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH + 1),
                Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH + 1),
                AuthorId = authorId
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync("api/news", newNewsRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

            badRequestModel.Should().NotBeNull();
            badRequestModel!.Errors.Should().NotBeEmpty();
            badRequestModel.Message.Should().Be(ValidationMessages.VALIDATION_MESSAGE_RESPONSE);
            badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        
        [Fact]
        public async Task CreateNewsAsync_ShouldReturnNotFoundRequest_WhenAuthorWasNotFound()
        {
            // Arrange
            await AuthenticateAsync();

            var authorId = Guid.Empty;
            var newNewsRequest = new NewNewsRequest
            {
                Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MINLENGTH + 1),
                Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MINLENGTH + 1),
                AuthorId = authorId
            };
            var exceptionMessage = new NotFoundException(nameof(Author), newNewsRequest.AuthorId).Message;

            // Act
            var response = await HttpClient.PostAsJsonAsync("api/news", newNewsRequest);

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
        
        #region CreateNewsAsync Tests

        [Fact]
        public async Task UpdateNewsAsync_ShouldReturnCreated_WhenValidationIsPassed()
        {
            // Arrange
            await AuthenticateAsync();

            var authorId = _dbContext.Authors.AsNoTracking().First().Id;
            var updateNewsRequest = new UpdateNewsRequest
            {
                Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH - 1),
                Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH - 1),
                AuthorId = authorId
            };

            // Act
            var response = await HttpClient.PutAsJsonAsync("api/news", updateNewsRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadFromJsonAsync<NewsResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().NotBe(Guid.Empty);
            responseContent.AuthorId.Should().Be(updateNewsRequest.AuthorId);
            responseContent.Content.Should().Be(updateNewsRequest.Content);
            responseContent.Subject.Should().Be(updateNewsRequest.Subject);
        }
        
        [Fact]
        public async Task UpdateNewsAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
        {
            // Arrange
            await AuthenticateAsync();

            var authorId = _dbContext.Authors.AsNoTracking().First().Id;
            var updateNewsRequest = new UpdateNewsRequest
            {
                Content = new Faker().Random.String2(ConfigurationConstants.CONTENT_MAXLENGTH + 1),
                Subject = new Faker().Random.String2(ConfigurationConstants.SUBJECT_MAXLENGTH + 1),
                AuthorId = authorId
            };

            // Act
            var response = await HttpClient.PutAsJsonAsync("api/news", updateNewsRequest);

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

        #region DeleteNewsAsync Tests

        [Fact]
        public async Task DeleteNewsAsync_ShouldReturnNoContent()
        {
            // Arrange
            var news = _dbContext.News.First();
        
            // Act
            var response = await HttpClient.DeleteAsync($"api/news/{news.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            _dbContext.News.AsNoTracking().Should().NotContainEquivalentOf(news, src => src.Excluding(t => t.CreatedAt));
        
            // Cleanup
            _dbContext.News.Add(news);
            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}