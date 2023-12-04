using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using System.Net.Http.Json;
using System.Net;
using NewsSite.DAL.DTO.Request.Rubric;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class RubricsControllerTests : BaseControllerTests
    {
        private readonly OnlineNewsContext _dbContext;

        public RubricsControllerTests(WebFactoryFixture fixture) : base(fixture)
        {
            _dbContext = fixture.DbContext;
        }

        [Fact]
        public async Task UpdateRubric_ShouldReturnOk()
        {
            // Arrange
            await AuthenticateAsync();

            var rubric = _dbContext.Rubrics.First();
            var rubricToUpdate = new UpdateRubricRequest
            {
                Id = rubric.Id,
                Name = $"{rubric.Name} {rubric.Name}"
            };

            // Act
            var response = await _httpClient.PutAsJsonAsync("api/rubrics", rubricToUpdate);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent =
                await response.Content.ReadFromJsonAsync<RubricResponse>();

            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(rubric.Id);
        }
    }
}
