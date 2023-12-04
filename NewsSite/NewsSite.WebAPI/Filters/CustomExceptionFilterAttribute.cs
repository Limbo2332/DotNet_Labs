using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NewsSite.DAL.DTO;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var httpStatusCode = context.Exception.ParseException();

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)httpStatusCode;

            context.Result = new JsonResult(new BadRequestModel
            {
                Errors = new List<string>
                {
                    context.Exception.Message,
                },
                HttpStatusCode = httpStatusCode,
                Message = context.Exception.Message
            });
        }
    }
}
