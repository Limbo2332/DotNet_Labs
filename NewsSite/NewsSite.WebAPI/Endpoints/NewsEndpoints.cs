using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Endpoints;

public static class NewsEndpoints
{
    public static void MapNewsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/news").AllowAnonymous();
        group.MapGet(string.Empty, GetNewsAsync);
        group.MapGet("by-rubric/{rubricId:guid}", GetNewsByRubricAsync);
        group.MapGet("by-tags", GetNewsByTagsAsync);
        group.MapGet("by-author/{authorId:guid}", GetNewsByAuthor);
        group.MapGet("{id:guid}", GetNewsByIdAsync);
        group.MapGet("by-date/{startDate:datetime}/{endDate:datetime}", GetNewsByPeriodOfTimeAsync);

        group.MapPost(string.Empty, CreateNewsAsync).RequireAuthorization();

        group.MapPut(string.Empty, UpdateNewsAsync).RequireAuthorization();

        group.MapDelete("{id:guid}", DeleteNewsAsync).RequireAuthorization();
    }

    private static async Task<IResult> GetNewsAsync(
        PageSettings? pageSettings,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByRubricAsync(
        PageSettings? pageSettings,
        [FromRoute] Guid rubricId,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByRubricAsync(rubricId, pageSettings);

        return Results.Ok(pageList);
    }
    
    private static async Task<IResult> GetNewsByTagsAsync(
        PageSettings? pageSettings,
        NewsByTagsRequest tagsIds,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByTagsAsync(tagsIds.TagsIds, pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByAuthor(
        PageSettings? pageSettings,
        [FromRoute] Guid authorId,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByAuthorAsync(authorId, pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByIdAsync(
        [FromRoute] Guid id,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByIdAsync(id);

        return Results.Ok(pageList);
    }

    private static async Task<IResult> GetNewsByPeriodOfTimeAsync(
        PageSettings? pageSettings,
        [FromRoute] DateTime startDate,
        [FromRoute] DateTime endDate,
        INewsService newsService)
    {
        var pageList = await newsService.GetNewsByPeriodOfTimeAsync(startDate, endDate, pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<Results<Created<NewsResponse>, NotFound<BadRequestModel>, BadRequest<BadRequestModel>>> CreateNewsAsync(
        [FromBody] NewNewsRequest newNewsRequest,
        INewsService newsService,
        IValidator<NewNewsRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(newNewsRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }

        try
        {
            var result = await newsService.CreateNewNewsAsync(newNewsRequest);

            return TypedResults.Created(nameof(CreateNewsAsync), result);
        }
        catch (NotFoundException ex)
        {
            return TypedResults.NotFound(ex.ToBadRequestModel());
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<Results<Ok<NewsResponse>, BadRequest<BadRequestModel>>> UpdateNewsAsync(
        [FromBody] UpdateNewsRequest updateNewsRequest,
        INewsService newsService,
        IValidator<UpdateNewsRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(updateNewsRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }
        
        try
        {
            var result = await newsService.UpdateNewsAsync(updateNewsRequest);

            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<IResult> DeleteNewsAsync(
        [FromRoute] Guid id,
        INewsService newsService)
    {
        await newsService.DeleteNewsAsync(id);

        return Results.NoContent();
    }
}