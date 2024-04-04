using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Endpoints;

public static class RubricsEndpoints
{
    public static void MapRubricsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/rubrics").RequireAuthorization();
        group.MapGet(string.Empty, GetRubricsAsync);
        group.MapGet("{id:guid}", GetRubricByIdAsync);
        group.MapPost(string.Empty, CreateNewRubricAsync);
        group.MapPost("newsRubrics", AddRubricForNewsAsync);
        group.MapPut(string.Empty, UpdateRubricAsync);
        group.MapDelete("{id:guid}", DeleteRubricAsync);
        group.MapDelete("newsRubrics/{rubricId:guid}/{newsId:guid}", DeleteRubricForNewsAsync);
    }

    private static async Task<IResult> GetRubricsAsync(
        PageSettings? pageSettings,
        IRubricsService rubricsService
    )
    {
        var pageList = await rubricsService.GetRubricsAsync(pageSettings);

        return Results.Ok(pageList);
    }
    
    private static async Task<Results<Ok<RubricResponse>, NotFound<BadRequestModel>>> GetRubricByIdAsync(
        [FromRoute] Guid id,
        IRubricsService rubricsService)
    {
        try
        {
            var rubric = await rubricsService.GetRubricByIdAsync(id);

            return TypedResults.Ok(rubric);
        }
        catch (Exception ex)
        {
            return TypedResults.NotFound(ex.ToBadRequestModel());
        }
    }

    private static async Task<Results<Created<RubricResponse>, BadRequest<BadRequestModel>>> CreateNewRubricAsync(
        [FromBody] NewRubricRequest newRubricRequest,
        IRubricsService rubricsService,
        IValidator<NewRubricRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(newRubricRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }

        try
        {
            var response = await rubricsService.CreateNewRubricAsync(newRubricRequest);

            return TypedResults.Created(nameof(CreateNewRubricAsync), response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<Results<Created<RubricResponse>, BadRequest<BadRequestModel>>> AddRubricForNewsAsync(
        NewsRubricRequest newRubricRequest,
        IRubricsService rubricsService)
    {
        try
        {
            var response = await rubricsService.AddRubricForNewsIdAsync(newRubricRequest);

            return TypedResults.Created(nameof(CreateNewRubricAsync), response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }
    
    private static async Task<Results<Ok<RubricResponse>, BadRequest<BadRequestModel>>> UpdateRubricAsync(
        [FromBody] UpdateRubricRequest updateRubricRequest,
        IRubricsService rubricsService,
        IValidator<UpdateRubricRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(updateRubricRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }
        
        try
        {
            var response = await rubricsService.UpdateRubricAsync(updateRubricRequest);

            return TypedResults.Ok(response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<NoContent> DeleteRubricAsync(
        [FromRoute] Guid id,
        IRubricsService rubricsService)
    {
        await rubricsService.DeleteRubricAsync(id);

        return TypedResults.NoContent();
    }

    private static async Task<NoContent> DeleteRubricForNewsAsync(
        [FromRoute] Guid rubricId,
        [FromRoute] Guid newsId,
        IRubricsService rubricsService)
    {
        await rubricsService.DeleteRubricForNewsIdAsync(rubricId, newsId);

        return TypedResults.NoContent();
    }
}