using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Controllers;
using System.Net;

namespace NewsSite.UnitTests.Systems.Controllers
{
    public class RubricsControllerTests
    {
        private readonly IRubricsService _rubricsService;
        private readonly RubricsController _sut;

        public RubricsControllerTests()
        {
            _rubricsService = Substitute.For<IRubricsService>();

            _sut = new RubricsController(_rubricsService);
        }

        [Fact]
        public async Task GetRubrics_ShouldBeSuccessful()
        {
            // Arrange
            var pageSettings = Substitute.For<PageSettings>();
            var rubricResponseList = Substitute.For<PageList<RubricResponse>>();

            _rubricsService
                .GetRubricsAsync(pageSettings)
                .Returns(rubricResponseList);

            // Act
            var result = await _sut.GetRubrics(pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(rubricResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetRubricById_ShouldBeSuccessful()
        {
            // Arrange
            var rubricId = Guid.Empty;
            var rubricResponse = Substitute.For<RubricResponse>();

            _rubricsService
                .GetRubricByIdAsync(rubricId)
                .Returns(rubricResponse);

            // Act
            var result = await _sut.GetRubricById(rubricId);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(rubricResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task CreateNewRubric_ShouldBeSuccessful()
        {
            // Arrange
            var newRubricRequest = Substitute.For<NewRubricRequest>();
            var rubricsResponse = Substitute.For<RubricResponse>();

            _rubricsService
                .CreateNewRubricAsync(newRubricRequest)
                .Returns(rubricsResponse);

            // Act
            var result = await _sut.CreateNewRubric(newRubricRequest);
            var response = result.Result as CreatedResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(rubricsResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task AddRubricForNews_ShouldBeSuccessful()
        {
            // Arrange
            var rubricId = Guid.Empty;
            var newsId = Guid.Empty;
            var rubricResponse = Substitute.For<RubricResponse>();

            _rubricsService
                .AddRubricForNewsIdAsync(rubricId, newsId)
                .Returns(rubricResponse);

            // Act
            var result = await _sut.AddRubricForNews(rubricId, newsId);
            var response = result.Result as CreatedResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(rubricResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task UpdateRubric_ShouldBeSuccessful()
        {
            // Arrange
            var updatedRubricRequest = Substitute.For<UpdateRubricRequest>();
            var rubricResponse = Substitute.For<RubricResponse>();

            _rubricsService
                .UpdateRubricAsync(updatedRubricRequest)
                .Returns(rubricResponse);

            // Act
            var result = await _sut.UpdateRubric(updatedRubricRequest);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(rubricResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }
        [Fact]
        public async Task DeleteRubric_ShouldBeSuccessful()
        {
            // Arrange
            var rubricId = Guid.Empty;

            _rubricsService
                .DeleteRubricAsync(rubricId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteRubric(rubricId);
            var response = result as NoContentResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task DeleteRubricForNews_ShouldBeSuccessful()
        {
            // Arrange
            var rubricId = Guid.Empty;
            var newsId = Guid.Empty;

            _rubricsService
                .DeleteRubricForNewsIdAsync(rubricId, newsId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteRubricForNews(rubricId, newsId);
            var response = result as NoContentResult;

            // Assert
            using (new AssertionScope())
            {
                _rubricsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            }
        }
    }
}
