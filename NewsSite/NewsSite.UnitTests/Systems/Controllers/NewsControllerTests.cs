using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Controllers;
using System.Net;

namespace NewsSite.UnitTests.Systems.Controllers
{
    public class NewsControllerTests
    {
        private readonly INewsService _newsService;
        private readonly NewsController _sut;

        public NewsControllerTests()
        {
            _newsService = Substitute.For<INewsService>();

            _sut = new NewsController(_newsService);
        }

        [Fact]
        public async Task GetNews_ShouldBeSuccessful()
        {
            // Arrange
            var pageSettings = Substitute.For<PageSettings>();
            var newsResponseList = Substitute.For<PageList<NewsResponse>>();

            _newsService
                .GetNewsAsync(pageSettings)
                .Returns(newsResponseList);

            // Act
            var result = await _sut.GetNews(pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetNewsByRubric_ShouldBeSuccessful()
        {
            // Arrange
            var rubricId = Guid.Empty;
            var pageSettings = Substitute.For<PageSettings>();
            var newsResponseList = Substitute.For<PageList<NewsResponse>>();

            _newsService
                .GetNewsByRubricAsync(rubricId, pageSettings)
                .Returns(newsResponseList);

            // Act
            var result = await _sut.GetNewsByRubric(rubricId, pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetNewsByTags_ShouldBeSuccessful()
        {
            // Arrange
            var tagsIds = Substitute.For<List<Guid>>();
            var pageSettings = Substitute.For<PageSettings>();
            var newsResponseList = Substitute.For<PageList<NewsResponse>>();

            _newsService
                .GetNewsByTagsAsync(tagsIds, pageSettings)
                .Returns(newsResponseList);

            // Act
            var result = await _sut.GetNewsByTags(tagsIds, pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetNewsByAuthor_ShouldBeSuccessful()
        {
            // Arrange
            var authorId = Guid.Empty;
            var pageSettings = Substitute.For<PageSettings>();
            var newsResponseList = Substitute.For<PageList<NewsResponse>>();

            _newsService
                .GetNewsByAuthorAsync(authorId, pageSettings)
                .Returns(newsResponseList);

            // Act
            var result = await _sut.GetNewsByAuthor(authorId, pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetNewsByPeriodOfTime_ShouldBeSuccessful()
        {
            // Arrange
            var startDate = DateTime.MinValue;
            var endDate = DateTime.MaxValue;
            var pageSettings = Substitute.For<PageSettings>();
            var newsResponseList = Substitute.For<PageList<NewsResponse>>();

            _newsService
                .GetNewsByPeriodOfTimeAsync(startDate, endDate, pageSettings)
                .Returns(newsResponseList);

            // Act
            var result = await _sut.GetNewsByPeriodOfTime(startDate, endDate, pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetNewsById_ShouldBeSuccessful()
        {
            // Arrange
            var newsId = Guid.Empty;
            var newsResponse = Substitute.For<NewsResponse>();

            _newsService
                .GetNewsByIdAsync(newsId)
                .Returns(newsResponse);

            // Act
            var result = await _sut.GetNewsById(newsId);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task CreateNewNews_ShouldBeSuccessful()
        {
            // Arrange
            var newNewsRequest = Substitute.For<NewNewsRequest>();
            var newsResponse = Substitute.For<NewsResponse>();

            _newsService
                .CreateNewNewsAsync(newNewsRequest)
                .Returns(newsResponse);

            // Act
            var result = await _sut.CreateNewNews(newNewsRequest);
            var response = result.Result as CreatedResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task UpdateNews_ShouldBeSuccessful()
        {
            // Arrange
            var updateNewsRequest = Substitute.For<UpdateNewsRequest>();
            var newsResponse = Substitute.For<NewsResponse>();

            _newsService
                .UpdateNewsAsync(updateNewsRequest)
                .Returns(newsResponse);

            // Act
            var result = await _sut.UpdateNews(updateNewsRequest);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(newsResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }
        [Fact]
        public async Task DeleteNews_ShouldBeSuccessful()
        {
            // Arrange
            var newsId = Guid.Empty;

            _newsService
                .DeleteNewsAsync(newsId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteNews(newsId);
            var response = result as NoContentResult;

            // Assert
            using (new AssertionScope())
            {
                _newsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            }
        }
    }
}
