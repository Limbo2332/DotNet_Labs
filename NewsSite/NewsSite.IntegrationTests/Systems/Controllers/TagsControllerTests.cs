using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Entities;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;
using Newtonsoft.Json;

namespace NewsSite.IntegrationTests.Systems.Controllers;

[Collection(nameof(WebFactoryFixture))]
public class TagsControllerTests(WebFactoryFixture fixture) : BaseControllerTests(fixture)
{
    private readonly OnlineNewsContext _dbContext = fixture.DbContext;
    
    #region GetTagsAsync Tests

    [Fact]
    public async Task GetTagsAsync_ShouldReturnOkResult()
    {
        // Act
        var response = await HttpClient.GetAsync("api/tags");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetTagsAsync_ShouldBeSorted_WhenPageSortingIsApplied()
    {
        // Arrange
        var query = new Dictionary<string, string?>
        {
            ["PageSorting.SortingProperty"] = "Name",
            ["PageSorting.SortingOrder"] = "desc"
        };

        var requestUri = QueryHelpers.AddQueryString("api/tags", query);

        // Act
        var response = await HttpClient.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().BeInDescendingOrder(i => i.Name);
    }
    
    [Fact]
    public async Task GetTagsAsync_ShouldBeFiltered_WhenPageFilteringIsApplied()
    {
        // Arrange
        var query = new Dictionary<string, string?>
        {
            ["PageFiltering.PropertyName"] = "name",
            ["PageFiltering.PropertyValue"] = "u"
        };

        var requestUri = QueryHelpers.AddQueryString("api/tags", query);

        // Act
        var response = await HttpClient.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Should().Contain(x => x.Name.Contains("u"));
    }
    
    [Fact]
    public async Task GetTagsAsync_ShouldBePaginated_WhenPagePaginationIsApplied()
    {
        // Arrange
        var query = new Dictionary<string, string?>
        {
            ["PagePagination.PageSize"] = "2",
            ["PagePagination.PageNumber"] = "3"
        };

        var requestUri = QueryHelpers.AddQueryString("api/tags", query);

        // Act
        var response = await HttpClient.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<PageList<TagResponse>>();

        responseContent.Should().NotBeNull();
        responseContent!.Items.Count.Should().Be(1);
    }

    #endregion

    #region GetTagByIdAsync Tests

    [Fact]
    public async Task GetTagByIdAsync_ShouldReturnOk()
    {
        // Arrange
        var tag = _dbContext.Tags.First();

        // Act
        var response = await HttpClient.GetAsync($"api/tags/{tag.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent.Should().BeEquivalentTo(tag, src => src.Excluding(t => t.CreatedAt));
    }
    
    [Fact]
    public async Task GetTagByIdAsync_ShouldReturnBadRequest_WhenTagNotFound()
    {
        // Arrange
        var tagId = Guid.Empty;
        var exceptionMessage = new NotFoundException(nameof(Tag), tagId).Message;

        // Act
        var response = await HttpClient.GetAsync($"api/tags/{tagId}");

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

    #region CreateNewTagAsync Tests

    [Fact]
    public async Task CreateNewTagAsync_ShouldReturnCreated_WhenNewTagIsCreated()
    {
        // Arrange
        await AuthenticateAsync();

        var newTagRequest = new NewTagRequest
        {
            Name = "Name"
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags", newTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().NotBe(Guid.Empty);
        responseContent.Name.Should().BeEquivalentTo(newTagRequest.Name);

        // Cleanup
        var tagToRemove = _dbContext.Tags.First(t => t.Id == responseContent.Id);
        _dbContext.Tags.Remove(tagToRemove);
        await _dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task CreateNewTagAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        await AuthenticateAsync();

        var newTagRequest = new NewTagRequest
        {
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags", newTagRequest);

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

    #region AddTagForNewsAsync Tests

    [Fact]
    public async Task AddTagForNewsAsync_ShouldReturnCreated_WhenTagIsAppliedForNews()
    {
        // Arrange
        await AuthenticateAsync();
        var tag = _dbContext.Tags.Last();

        var newNewsTagRequest = new NewsTagRequest
        {
            TagId = tag.Id,
            NewsId = _dbContext.News.Last().Id
        };

        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags/newsTag", newNewsTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent.Should().BeEquivalentTo(tag, src => src.Excluding(t => t.CreatedAt));
        
        // Cleanup
        var newsTag = _dbContext.NewsTags.First(nt => nt.TagId == tag.Id && nt.NewsId == newNewsTagRequest.NewsId);
        _dbContext.NewsTags.Remove(newsTag);
        await _dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task AddTagForNewsAsync_ShouldReturnBadRequest_WhenTagIsAlreadyAppliedForNews()
    {
        // Arrange
        await AuthenticateAsync();

        var newNewsTagRequest = new NewsTagRequest
        {
            TagId = _dbContext.Tags.First().Id,
            NewsId = _dbContext.News.First().Id
        };
        var exceptionMessage = new NotFoundException(
            "Tag",
            newNewsTagRequest.TagId,
            "News",
            newNewsTagRequest.NewsId).Message;
        await HttpClient.PostAsJsonAsync("api/tags/newsTag", newNewsTagRequest);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("api/tags/newsTag", newNewsTagRequest);

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

    #region UpdateTagAsync Tests

    [Fact]
    public async Task UpdateTagAsync_ShouldReturnOk_WhenTagExists()
    {
        // Arrange
        await AuthenticateAsync();
        var tag = _dbContext.Tags.First();

        var newNewsTagRequest = new UpdateTagRequest
        {
            Id = tag.Id,
            Name = "Name"
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/tags", newNewsTagRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<TagResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Name.Should().Be("Name");
        
        // Cleanup
        var newsTag = _dbContext.Tags.First(t => t.Id == tag.Id);
        newsTag.Name = tag.Name;
        _dbContext.Tags.Update(newsTag);
        await _dbContext.SaveChangesAsync();
    }
    
    [Fact]
    public async Task UpdateTagAsync_ShouldReturnBadRequest_WhenValidationIsNotPassed()
    {
        // Arrange
        await AuthenticateAsync();

        var newTagRequest = new UpdateTagRequest
        {
            Id = _dbContext.Tags.First().Id,
            Name = string.Empty
        };

        // Act
        var response = await HttpClient.PutAsJsonAsync("api/tags", newTagRequest);

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

    #region DeleteTagAsync Tests

    [Fact]
    public async Task DeleteTagAsync_ShouldReturnNoContent()
    {
        // Arrange
        var tag = _dbContext.Tags.First();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/tags/{tag.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _dbContext.Tags.AsNoTracking().Should().NotContainEquivalentOf(tag, src => src.Excluding(t => t.CreatedAt));
        
        // Cleanup
        _dbContext.Tags.Add(tag);
        await _dbContext.SaveChangesAsync();
    }

    #endregion

    #region DeleteTagForNewsAsync Tests

    [Fact]
    public async Task DeleteTagForNewsAsync_ShouldReturnNoContent()
    {
        // Arrange
        await AuthenticateAsync();
        var newsTag = _dbContext.NewsTags.First();
        
        // Act
        var response = await HttpClient.DeleteAsync($"api/tags/newsTag/{newsTag.TagId}/{newsTag.NewsId}");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _dbContext.NewsTags.Should().NotContain(newsTag);
        
        // Cleanup
        await _dbContext.NewsTags.AddAsync(newsTag);
        await _dbContext.SaveChangesAsync();
    }

    #endregion
}