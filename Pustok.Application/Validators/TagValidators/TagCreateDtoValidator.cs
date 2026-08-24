using FluentValidation;
using Pustok.Application.Dtos.TagDtos;

namespace Pustok.Application.Validators.TagValidators;

public class TagCreateDtoValidator : AbstractValidator<TagCreateDto>
{
    public TagCreateDtoValidator()
    {
        RuleFor(x => x.Name).Must(x=>!string.IsNullOrWhiteSpace(x)).WithMessage("Name is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
    }
}
