using System.Net;
using FluentValidation.Results;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO;

namespace NewsSite.UI.Extensions;

public static class FluentValidationExtensions
{
    public static BadRequestModel ToBadRequestModel(this ValidationResult validationResult)
    {
        var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

        return new BadRequestModel
        {
            Errors = errors,
            HttpStatusCode = HttpStatusCode.BadRequest,
            Message = ValidationMessages.VALIDATION_MESSAGE_RESPONSE
        };
    }
}