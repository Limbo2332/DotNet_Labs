using FluentValidation;
using NewsSite.BLL.Interfaces;
using NewsSite.DAL.Constants;
using NewsSite.DAL.Context.Constants;
using NewsSite.DAL.DTO.Request.Author;
using NewsSite.UI.Extensions;

namespace NewsSite.UI.Validators.Request.Author
{
    public class UpdatedAuthorRequestValidator : AbstractValidator<UpdatedAuthorRequest>
    {
        public UpdatedAuthorRequestValidator(IAuthorsService authorsService)
        {
            RuleFor(ua => ua.Email)
                .CustomEmail()
                .Must(authorsService.IsEmailUnique)
                    .WithMessage(ValidationMessages.GetEntityIsNotUniqueMessage(ValidationMessages.EMAIL_PROPERTY_NAME));

            RuleFor(ua => ua.FullName)
                .CustomFullName();

            RuleFor(ur => ur.BirthDate)
                .LessThan(DateTime.UtcNow.AddYears(-ConfigurationConstants.MIN_YEARS_TO_REGISTER))
                .WithMessage(ValidationMessages.BirthDateLessThanYears);
        }
    }
}
