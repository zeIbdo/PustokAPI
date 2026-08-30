using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").EmailAddress();
        RuleFor(x => x.Username).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty");
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").MinimumLength(3).WithMessage("Password min 3 char length");
        RuleFor(x=>x.ConfirmPassword).NotEmpty().Equal(x=>x.Password).WithMessage("Passwords do not match");
    }
}
