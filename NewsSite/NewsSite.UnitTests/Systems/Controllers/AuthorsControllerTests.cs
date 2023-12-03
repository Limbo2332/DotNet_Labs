using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.UI.Controllers;
using System.Net;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.UnitTests.Systems.Controllers
{
    public class AuthorsControllerTests
    {
        private readonly IAuthorsService _authorsService;
        private readonly AuthorsController _sut;

        public AuthorsControllerTests()
        {
            _authorsService = Substitute.For<IAuthorsService>();

            _sut = new AuthorsController(_authorsService);
        }

        [Fact]
        public async Task GetAuthors_ShouldBeSuccessful()
        {
            // Arrange
            var pageSettings = Substitute.For<PageSettings>();
            var authorResponseList = Substitute.For<PageList<AuthorResponse>>();

            _authorsService
                .GetAuthorsAsync(pageSettings)
                .Returns(authorResponseList);

            // Act
            var result = await _sut.GetAuthors(pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _authorsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(authorResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetAuthorById_ShouldBeSuccessful()
        {
            // Arrange
            var authorId = Guid.Empty;
            var authorResponse = Substitute.For<AuthorResponse>();

            _authorsService
                .GetAuthorByIdAsync(authorId)
                .Returns(authorResponse);

            // Act
            var result = await _sut.GetAuthorById(authorId);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _authorsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(authorResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task UpdateAuthor_ShouldBeSuccessful()
        {
            // Arrange
            var updatedAuthorRequest = Substitute.For<UpdatedAuthorRequest>();
            var authorResponse = Substitute.For<AuthorResponse>();

            _authorsService
                .UpdateAuthorAsync(updatedAuthorRequest)
                .Returns(authorResponse);

            // Act
            var result = await _sut.UpdateAuthor(updatedAuthorRequest);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _authorsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(authorResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }
        [Fact]
        public async Task DeleteAuthor_ShouldBeSuccessful()
        {
            // Arrange
            var authorId = Guid.Empty;

            _authorsService
                .DeleteAuthorAsync(authorId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteAuthor(authorId);
            var response = result as NoContentResult;

            // Assert
            using (new AssertionScope())
            {
                _authorsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            }
        }
    }
}
