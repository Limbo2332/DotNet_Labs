using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO;

namespace NewsSite.UI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

                context.Result = new BadRequestObjectResult(new BadRequestModel
                {
                    Errors = errors,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    Message = ValidationMessages.VALIDATION_MESSAGE_RESPONSE
                });
            }
        }
    }
}
