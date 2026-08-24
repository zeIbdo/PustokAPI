using FluentValidation;
using Pustok.Application.Dtos.TagDtos;

namespace Pustok.Application.Validators.TagValidators;

public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
{
    public TagUpdateDtoValidator()
    {
        RuleFor(x => x.Name).Must(x => x == null || !string.IsNullOrWhiteSpace(x))
    .WithMessage("Name cannot be empty or whitespace")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
    }
}
