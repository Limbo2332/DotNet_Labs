using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Endpoints;

public static class AuthorsEndpoints
{
    public static void MapAuthorsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/authors");
        
        group.MapGet(string.Empty, GetAuthorsAsync).AllowAnonymous();
        group.MapGet("{id:guid}", GetAuthorByIdAsync).AllowAnonymous();

        group.MapPut(string.Empty, UpdateAuthorAsync).RequireAuthorization();

        group.MapDelete("{id:guid}", DeleteAuthorAsync).RequireAuthorization();
    }

    private static async Task<IResult> GetAuthorsAsync(
        PageSettings? pageSettings,
        IAuthorsService authorsService)
    {
        var pageList = await authorsService.GetAuthorsAsync(pageSettings);

        return Results.Ok(pageList);
    }

    private static async Task<Results<Ok<AuthorResponse>, NotFound<BadRequestModel>, BadRequest<BadRequestModel>>> GetAuthorByIdAsync(
        [FromRoute] Guid id,
        IAuthorsService authorsService)
    {
        try
        {
            var author = await authorsService.GetAuthorByIdAsync(id);

            return TypedResults.Ok(author);
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

    private static async Task<Results<Ok<AuthorResponse>, BadRequest<BadRequestModel>>> UpdateAuthorAsync(
        [FromBody] UpdatedAuthorRequest updatedAuthor,
        IAuthorsService authorsService,
        IValidator<UpdatedAuthorRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(updatedAuthor);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }
        
        try
        {
            var result = await authorsService.UpdateAuthorAsync(updatedAuthor);

            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }

    private static async Task<IResult> DeleteAuthorAsync(
        [FromRoute] Guid id,
        IAuthorsService authorsService)
    {
        await authorsService.DeleteAuthorAsync(id);

        return Results.NoContent();
    }
}