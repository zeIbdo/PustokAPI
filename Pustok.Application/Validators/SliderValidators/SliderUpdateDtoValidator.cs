using FluentValidation;
using Pustok.Application.Dtos.SliderDtos;

namespace Pustok.Application.Validators.SliderValidators;

public class SliderUpdateDtoValidator : AbstractValidator<SliderUpdateDto>
{
    public SliderUpdateDtoValidator()
    {
        RuleFor(x => x.Title).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Title cannot be empty or whitespace").MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Description).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be empty or whitespace").MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Price).GreaterThan(0m).WithMessage("Must be greater than 0").LessThanOrEqualTo(999999.99m).WithMessage("Price must be between 0.01 and 999999.99");
        RuleFor(x => x.Image).Must(x => x == null || x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x == null || x.ContentType.StartsWith("image/")).WithMessage("Must be image");
    }
}
