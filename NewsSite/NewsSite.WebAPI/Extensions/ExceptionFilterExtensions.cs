using NewsSite.BLL.Exceptions;
using NewsSite.DAL.Enums;
using System.Net;

namespace NewsSite.UI.Extensions
{
    public static class ExceptionFilterExtensions
    {
        public static (HttpStatusCode statusCode, ErrorCode errorCode) ParseException(this Exception exception)
        {
            return exception switch
            {
                BadRequestException => (HttpStatusCode.BadRequest, ErrorCode.BadRequest),
                NotFoundException => (HttpStatusCode.NotFound, ErrorCode.NotFound),
                InvalidEmailOrPasswordException => (HttpStatusCode.BadRequest, ErrorCode.InvalidEmailOrPassword),
                _ => (HttpStatusCode.InternalServerError, ErrorCode.General)
            };
        }
    }
}
