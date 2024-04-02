using NewsSite.DAL.DTO;

namespace NewsSite.UI.Extensions;

public static class ExceptionExtensions
{
    public static BadRequestModel ToBadRequestModel(this Exception exception)
    {
        var httpStatusCode = exception.ParseException();

        return new BadRequestModel
        {
            Errors = [exception.Message],
            HttpStatusCode = httpStatusCode,
            Message = exception.Message
        };
    }
}