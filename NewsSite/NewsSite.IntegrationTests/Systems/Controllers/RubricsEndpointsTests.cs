using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.Entities;
using NewsSite.UnitTests.TestData;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Controllers
{
    [Collection(nameof(WebFactoryFixture))]
    public class RubricsEndpointsTests(WebFactoryFixture fixture) : BaseControllerTests(fixture)
    {
    private readonly OnlineNewsContext _dbContext = fixture.DbContext;
    
    #region GetRubricsAsync Tests

    [Fact]
    public async Task GetRubricsAsync_ShouldReturnOkResult()
    {
        // Act
        var response = await HttpClient.GetAsync("api/rubrics");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetRubricsAsync_ShouldBeSorted_WhenPageSortingIsApplied()
    {
        // Arrange
        var query = new Dictionary<string, string?>
        {
            ["PageSorting.SortingProperty"] = "Name",
            ["PageSorting.SortingOrder"] = "desc"
        };

        var requestUri = QueryHelpers.AddQueryString("api/rubrics", query);

        // Act
        var response = await HttpClient.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.Name);
    }
    
    [Fact]
    public async Task GetRubricsAsync_ShouldBeFiltered_WhenPageFilteringIsApplied()
    {
        // Arrange
        var query = new Dictionary<string, string?>
        {
            ["PageFiltering.PropertyName"] = "name",
            ["PageFiltering.PropertyValue"] = "u"
        };

        var requestUri = QueryHelpers.AddQueryString("api/rubrics", query);

        // Act
        var response = await HttpClient.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().Contain(x => x.Name.Contains("u"));
    }
    
    [Fact]
    public async Task GetRubricsAsync_ShouldBePaginated_WhenPagePaginationIsApplied()
    {
        // Arrange
        var query = new Dictionary<string, string?>
        {
            ["PagePagination.PageSize"] = "2",
            ["PagePagination.PageNumber"] = "3"
        };

        var requestUri = QueryHelpers.AddQueryString("api/rubrics", query);

        // Act
        var response = await HttpClient.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<RubricResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Count.Should().Be(1);
    }

    #endregion

    #region GetRubricByIdAsync Tests

    [Fact]
    public async Task GetRubricByIdAsync_ShouldReturnOk()
    {
        // Arrange
        var rubric = _dbContext.Rubrics.First();

        // Act
        var response = await HttpClient.GetAsync($"api/rubrics/{rubric.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        responseContent.Should().BeEquivalentTo(rubric, src => src.Excluding(t => t.CreatedAt));
    }
    
    [Fact]
    public async Task GetRubricByIdAsync_ShouldReturnBadRequest_WhenRubricNotFound()
    {
        // Arrange
        var rubricId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Rubric), rubricId).Message;

        // Act
        var response = await HttpClient.GetAsync($"api/rubrics/{rubricId}");

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

    #region CreateNewRubricAsync Tests

    [Fact]
    public async Task CreateNewRubricAsync_ShouldReturnCreated_WhenNewRubricIsCreated()
    {
        // Arrange
        await AuthenticateAsync();

        var newRubricRequest = new NewRubricRequest
        {
            Name = "NameNameName"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics", newRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.Name.Should().BeEquivalentTo(newRubricRequest.Name);

        // Cleanup
        var rubricToRemove = _dbContext.Rubrics.First(t => t.Id == responseContent.Id);
        _dbContext.Rubrics.Remove(rubricToRemove);
        await _dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task CreateNewRubricAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        await AuthenticateAsync();

        var newRubricRequest = new NewRubricRequest
        {
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics", newRubricRequest);

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

    #region AddRubricForNewsAsync Tests

    [Fact]
    public async Task AddRubricForNewsAsync_ShouldReturnCreated_WhenRubricIsAppliedForNews()
    {
        // Arrange
        await AuthenticateAsync();
        var newsRubric = RepositoriesFakeData.GenerateNewsRubrics(1, 1).First();
        var newNewsRubricRequest = new NewsRubricRequest
        {
            RubricId = newsRubric.RubricId,
            NewsId = newsRubric.NewsId
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics/newsRubrics", newNewsRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        _dbContext.NewsRubrics.AsNoTracking().ToList().Should().ContainEquivalentOf(newsRubric);
        
        // Cleanup
        var newsRubrics = _dbContext.NewsRubrics.AsNoTracking().First(nt => nt.RubricId == newsRubric.RubricId && nt.NewsId == newNewsRubricRequest.NewsId);
        _dbContext.NewsRubrics.Remove(newsRubrics);
        await _dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task AddRubricForNewsAsync_ShouldReturnBadRequest_WhenRubricIsAlreadyAppliedForNews()
    {
        // Arrange
        await AuthenticateAsync();

        var newNewsRubricRequest = new NewsRubricRequest
        {
            RubricId = _dbContext.Rubrics.First().Id,
            NewsId = _dbContext.News.First().Id
        };
        var exceptionMessage = new NotFoundException(
            "Rubric",
            newNewsRubricRequest.RubricId,
            "News",
            newNewsRubricRequest.NewsId).Message;
        await HttpClient.PostAsJsonAsync("api/rubrics/newsRubrics", newNewsRubricRequest);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/rubrics/newsRubrics", newNewsRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseContent = await response.Content.ReadAsStringAsync();
        var badRequestModel = JsonConvert.DeserializeObject<BadRequestModel>(responseContent);

        badRequestModel.Should().NotBeNull();
        badRequestModel!.Errors.Should().NotBeEmpty();
        badRequestModel.Message.Should().Be(exceptionMessage);
        badRequestModel.HttpStatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region UpdateRubricAsync Tests

    [Fact]
    public async Task UpdateRubricAsync_ShouldReturnOk_WhenRubricExists()
    {
        // Arrange
        await AuthenticateAsync();
        var rubric = _dbContext.Rubrics.First();

        var newNewsRubricRequest = new UpdateRubricRequest
        {
            Id = rubric.Id,
            Name = "NameNameName"
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/rubrics", newNewsRubricRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<RubricResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Name.Should().Be("NameNameName");
        
        // Cleanup
        var newsRubrics = _dbContext.Rubrics.First(t => t.Id == rubric.Id);
        newsRubrics.Name = rubric.Name;
        _dbContext.Rubrics.Update(newsRubrics);
        await _dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task UpdateRubricAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        await AuthenticateAsync();

        var newRubricRequest = new UpdateRubricRequest
        {
            Id = _dbContext.Rubrics.First().Id,
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/rubrics", newRubricRequest);

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

    #region DeleteRubricAsync Tests

    [Fact]
    public async Task DeleteRubricAsync_ShouldReturnNoContent()
    {
        // Arrange
        var rubric = _dbContext.Rubrics.First();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/rubrics/{rubric.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _dbContext.Rubrics.AsNoTracking().Should().NotContainEquivalentOf(rubric, src => src.Excluding(t => t.CreatedAt));
        
        // Cleanup
        _dbContext.Rubrics.Add(rubric);
        await _dbContext.SaveChangesAsync();
    }

    #endregion

    #region DeleteRubricForNewsAsync Tests

    [Fact]
    public async Task DeleteRubricForNewsAsync_ShouldReturnNoContent()
    {
        // Arrange
        await AuthenticateAsync();
        var newsRubrics = _dbContext.NewsRubrics.First();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/rubrics/newsRubrics/{newsRubrics.RubricId}/{newsRubrics.NewsId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _dbContext.NewsRubrics.Should().NotContain(newsRubrics);
        
        // Cleanup
        await _dbContext.NewsRubrics.AddAsync(newsRubrics);
        await _dbContext.SaveChangesAsync();
    }

    #endregion
    }
}
