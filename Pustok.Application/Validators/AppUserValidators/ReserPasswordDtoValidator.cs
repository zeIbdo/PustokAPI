using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class ReserPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ReserPasswordDtoValidator()
    {
        RuleFor(x => x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).EmailAddress();
        RuleFor(x => x.Token).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty");
        RuleFor(x => x.Password).Cascade(CascadeMode.Stop).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Cannot be empty").MinimumLength(3).WithMessage("Password min 3 char length");
        RuleFor(x => x.ConfirmPassword).NotEmpty().Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}
