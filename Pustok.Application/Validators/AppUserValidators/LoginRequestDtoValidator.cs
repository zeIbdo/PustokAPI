using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestDtoValidator()
    {
        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").EmailAddress();
        RuleFor(x => x.Password).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty");
    }
}
