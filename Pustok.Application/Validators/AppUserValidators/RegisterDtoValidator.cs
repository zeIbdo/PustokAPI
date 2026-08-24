using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").EmailAddress();
        RuleFor(x => x.Username).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty");
        RuleFor(x => x.Password).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").MinimumLength(3).WithMessage("Password min 3 char length");
        RuleFor(x=>x.ConfirmPassword).Equal(x=>x.Password).WithMessage("Passwords do not match");
    }
}
