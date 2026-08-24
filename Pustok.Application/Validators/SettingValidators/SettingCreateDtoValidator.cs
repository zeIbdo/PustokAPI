using FluentValidation;
using Pustok.Application.Dtos.SettingDtos;

namespace Pustok.Application.Validators.SettingValidators;

public class SettingCreateDtoValidator : AbstractValidator<SettingCreateDto>
{
    public SettingCreateDtoValidator()
    {
        RuleFor(x => x.Key).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Key is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Value).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Value is required")
            .MaximumLength(1024).WithMessage("Cannot exceed 1024 chars");
    }
}
