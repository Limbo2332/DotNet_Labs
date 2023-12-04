using NewsSite.BLL.Exceptions;
using System.Net;

namespace NewsSite.UI.Extensions
{
    public static class ExceptionFilterExtensions
    {
        public static HttpStatusCode ParseException(this Exception exception)
        {
            return exception switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                InvalidEmailOrPasswordException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
