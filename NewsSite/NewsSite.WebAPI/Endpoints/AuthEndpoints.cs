using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.DTO;
using NewsSite.DAL.DTO.Request.Auth;
using NewsSite.DAL.DTO.Response;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth");
        group.MapPost(nameof(Login), Login);
        group.MapPost(nameof(Register), Register);
    }

    private static async Task<Results<Ok<LoginUserResponse>, NotFound<BadRequestModel>, BadRequest<BadRequestModel>>> Login(
        [FromBody] UserLoginRequest userLoginRequest, 
        IAuthService authService,
        IValidator<UserLoginRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(userLoginRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }

        try
        {
            var response = await authService.LoginAsync(userLoginRequest);

            return TypedResults.Ok(response);
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

    private static async Task<Results<Created<NewUserResponse>, BadRequest<BadRequestModel>>> Register(
        [FromBody] UserRegisterRequest userRegisterRequest,
        IAuthService authService,
        IValidator<UserRegisterRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(userRegisterRequest);
        if (!validationResult.IsValid)
        {
            return TypedResults.BadRequest(validationResult.ToBadRequestModel());
        }

        try
        {
            var response = await authService.RegisterAsync(userRegisterRequest);

            return TypedResults.Created(nameof(Register), response);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.ToBadRequestModel());
        }
    }
}