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
                BadRequestException _ => (HttpStatusCode.BadRequest, ErrorCode.BadRequest),
                NotFoundException _ => (HttpStatusCode.NotFound, ErrorCode.NotFound),
                InvalidEmailOrPasswordException _ => (HttpStatusCode.BadRequest, ErrorCode.InvalidEmailOrPassword),
                _ => (HttpStatusCode.InternalServerError, ErrorCode.General)
            };
        }
    }
}
