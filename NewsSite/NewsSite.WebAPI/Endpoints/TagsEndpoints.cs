using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Tag;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Endpoints;

public static class TagsEndpoints
{
    public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/tags");
        group.MapGet(string.Empty, GetTagsAsync).AllowAnonymous();
        group.MapGet("{id:guid}", GetTagByIdAsync);
        group.MapPost(string.Empty, CreateNewTagAsync);
        group.MapPost("newsTag", AddTagForNewsAsync);
        group.MapPut(string.Empty, UpdateTagAsync);
        group.MapDelete("{id:guid}", DeleteTagAsync);
        group.MapDelete("newsTag/{tagId:guid}/{newsId:guid}", DeleteTagForNewsAsync);
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

    private static async Task<Results<Created<TagResponse>, BadRequest<BadRequestModel>>> CreateNewTagAsync(
        [FromBody] NewTagRequest newTagRequest,
        ITagsService tagsService,
        IValidator<NewTagRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(newTagRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }

        try
        {
            var response = await tagsService.CreateNewTagAsync(newTagRequest);

            return TypedResults.Created(nameof(CreateNewTagAsync), response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<Results<Created<TagResponse>, BadRequest<BadRequestModel>>> AddTagForNewsAsync(
        NewsTagRequest newsTagRequest,
        ITagsService tagsService)
    {
        try
        {
            var response = await tagsService.AddTagForNewsIdAsync(newsTagRequest);

            return TypedResults.Created(nameof(CreateNewTagAsync), response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }
    
    private static async Task<Results<Ok<TagResponse>, BadRequest<BadRequestModel>>> UpdateTagAsync(
        [FromBody] UpdateTagRequest updateTagRequest,
        ITagsService tagsService,
        IValidator<UpdateTagRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(updateTagRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }
        
        try
        {
            var response = await tagsService.UpdateTagAsync(updateTagRequest);

            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<NoContent> DeleteTagAsync(
        [FromRoute] Guid id,
        ITagsService tagsService)
    {
        await tagsService.DeleteTagAsync(id);

        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteTagForNewsAsync(
        [FromRoute] Guid tagId,
        [FromRoute] Guid newsId,
        ITagsService tagsService)
    {
        await tagsService.DeleteTagForNewsIdAsync(tagId, newsId);

        return TypedResults.NoContent();
    }
}