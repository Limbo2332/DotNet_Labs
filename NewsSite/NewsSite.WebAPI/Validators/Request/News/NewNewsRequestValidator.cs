using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.DTO.Request.News;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Request.News
{ 
    public class NewNewsRequestValidator : AbstractValidator<NewNewsRequest>
    {
        public NewNewsRequestValidator()
        {
            RuleFor(nn => nn.Subject)
                .CustomSubject();

            RuleFor(nn => nn.Content)
                .CustomContent();
        }
    }
}
