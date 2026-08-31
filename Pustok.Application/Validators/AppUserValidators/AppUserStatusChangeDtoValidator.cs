using FluentValidation;
using Pustok.Application.Dtos.AppUserDtos;

namespace Pustok.Application.Validators.AppUserValidators;

public class AppUserStatusChangeDtoValidator : AbstractValidator<AppUserStatusChangeDto>
{
    public AppUserStatusChangeDtoValidator()
    {
    }
}
