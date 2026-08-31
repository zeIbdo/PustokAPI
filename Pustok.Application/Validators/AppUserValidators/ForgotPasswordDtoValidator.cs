using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordDtoValidator()
    {
        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").EmailAddress().WithMessage("Must be email address");
    }
}
