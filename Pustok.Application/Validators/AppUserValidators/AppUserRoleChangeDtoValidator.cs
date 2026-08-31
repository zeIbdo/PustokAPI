using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class AppUserRoleChangeDtoValidator : AbstractValidator<AppUserRoleChangeDto>
{
    public AppUserRoleChangeDtoValidator()
    {
        RuleFor(x => x.Role).NotEmpty().WithMessage("Cant be empty");
    }
}
