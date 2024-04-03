using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response;
using NewsSite.IntegrationTests.Fixtures;
using NewsSite.IntegrationTests.Systems.Controllers.Abstract;

namespace NewsSite.IntegrationTests.Systems.Controllers;

[Collection(nameof(WebFactoryFixture))]
public class TagsControllerTests(WebFactoryFixture fixture) : BaseControllerTests(fixture)
{
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
    
    [Fact]
    public async Task GetAuthorById_ShouldReturnOk()
    {
        // Arrange
        var author = _dbContext.Authors.First();

        // Act
        var response = await HttpClient.GetAsync($"api/authors/{author.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseContent = await response.Content.ReadFromJsonAsync<AuthorResponse>();

        responseContent.Should().NotBeNull();
        responseContent!.Id.Should().Be(author.Id);
    }
}