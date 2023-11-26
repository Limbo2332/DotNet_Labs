using FluentValidation;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Request.Rubric;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Request.Rubric
{
    public class NewRubricRequestValidator : AbstractValidator<NewRubricRequest>
    {
        public NewRubricRequestValidator()
        {
            RuleFor(nr => nr.Name)
                .CustomRubricName();
        }
    }
}
