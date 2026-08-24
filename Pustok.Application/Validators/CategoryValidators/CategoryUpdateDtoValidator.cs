using FluentValidation;
using Pustok.Application.Dtos.CategoryDtos;

namespace Pustok.Application.Validators.CategoryValidators;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Name).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Title cannot be empty or whitespace").MaximumLength(256).WithMessage("Cannot exceed 256 chars");
    }
}
