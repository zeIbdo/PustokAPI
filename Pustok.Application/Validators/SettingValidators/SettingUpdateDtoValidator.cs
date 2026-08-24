using FluentValidation;
using Pustok.Application.Dtos.SettingDtos;

namespace Pustok.Application.Validators.SettingValidators;

public class SettingUpdateDtoValidator : AbstractValidator<SettingUpdateDto>
{
    public SettingUpdateDtoValidator()
    {
        RuleFor(x => x.Key).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Key cannot be empty or whitespace")
           .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Value).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Value cannot be empty or whitespace")
            .MaximumLength(1024).WithMessage("Cannot exceed 1024 chars");
    }
}
