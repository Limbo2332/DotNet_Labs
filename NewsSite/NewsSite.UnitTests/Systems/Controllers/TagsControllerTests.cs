using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Controllers;
using System.Net;

namespace NewsSite.UnitTests.Systems.Controllers
{
    public class TagsControllerTests
    {
        private readonly ITagsService _tagsService;
        private readonly TagsController _sut;

        public TagsControllerTests()
        {
            _tagsService = Substitute.For<ITagsService>();

            _sut = new TagsController(_tagsService);
        }

        [Fact]
        public async Task GetTags_ShouldBeSuccessful()
        {
            // Arrange
            var pageSettings = Substitute.For<PageSettings>();
            var tagResponseList = Substitute.For<PageList<TagResponse>>();

            _tagsService
                .GetTagsAsync(pageSettings)
                .Returns(tagResponseList);

            // Act
            var result = await _sut.GetTags(pageSettings);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(tagResponseList);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task GetTagById_ShouldBeSuccessful()
        {
            // Arrange
            var tagId = Guid.Empty;
            var tagResponse = Substitute.For<TagResponse>();

            _tagsService
                .GetTagByIdAsync(tagId)
                .Returns(tagResponse);

            // Act
            var result = await _sut.GetTagById(tagId);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(tagResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task CreateNewTag_ShouldBeSuccessful()
        {
            // Arrange
            var newTagRequest = Substitute.For<NewTagRequest>();
            var tagsResponse = Substitute.For<TagResponse>();

            _tagsService
                .CreateNewTagAsync(newTagRequest)
                .Returns(tagsResponse);

            // Act
            var result = await _sut.CreateNewTag(newTagRequest);
            var response = result.Result as CreatedResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(tagsResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task AddTagForNews_ShouldBeSuccessful()
        {
            // Arrange
            var tagId = Guid.Empty;
            var newsId = Guid.Empty;
            var tagResponse = Substitute.For<TagResponse>();

            _tagsService
                .AddTagForNewsIdAsync(tagId, newsId)
                .Returns(tagResponse);

            // Act
            var result = await _sut.AddTagForNews(tagId, newsId);
            var response = result.Result as CreatedResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(tagResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task UpdateTag_ShouldBeSuccessful()
        {
            // Arrange
            var updatedTagRequest = Substitute.For<UpdateTagRequest>();
            var tagResponse = Substitute.For<TagResponse>();

            _tagsService
                .UpdateTagAsync(updatedTagRequest)
                .Returns(tagResponse);

            // Act
            var result = await _sut.UpdateTag(updatedTagRequest);
            var response = result.Result as OkObjectResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.Value.Should().Be(tagResponse);
                response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            }
        }
        [Fact]
        public async Task DeleteTag_ShouldBeSuccessful()
        {
            // Arrange
            var tagId = Guid.Empty;

            _tagsService
                .DeleteTagAsync(tagId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteTag(tagId);
            var response = result as NoContentResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public async Task DeleteTagForNews_ShouldBeSuccessful()
        {
            // Arrange
            var tagId = Guid.Empty;
            var newsId = Guid.Empty;

            _tagsService
                .DeleteTagForNewsIdAsync(tagId, newsId)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _sut.DeleteTagForNews(tagId, newsId);
            var response = result as NoContentResult;

            // Assert
            using (new AssertionScope())
            {
                _tagsService.ReceivedCalls().Count().Should().Be(1);

                response.Should().NotBeNull();
                response!.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            }
        }
    }
}
