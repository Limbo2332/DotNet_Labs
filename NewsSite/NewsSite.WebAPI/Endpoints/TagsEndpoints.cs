using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO.Page;

namespace NewsSite.UI.Endpoints;

public static class TagsEndpoints
{
    public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tags");
        group.MapGet(string.Empty, GetTagsAsync).AllowAnonymous();
        group.MapGet("{id:guid}", GetTagByIdAsync);
    }

    private static async Task<IResult> GetTagsAsync(
        PageSettings pageSettings,
        ITagsService tagsService)
    {
        var pageList = await tagsService.GetTagsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetTagByIdAsync(
        [FromRoute] Guid id,
        ITagsService tagsService)
    {
        var tag = await tagsService.GetTagByIdAsync(id);

        return Results.Ok(tag);
    }
}