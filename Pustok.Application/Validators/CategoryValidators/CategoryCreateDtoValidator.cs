using FluentValidation;
using Pustok.Application.Dtos.CategoryDtos;

namespace Pustok.Application.Validators.CategoryValidators;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(x=>x.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Name cannot be empty").MaximumLength(256).WithMessage("max 256 chars");
    }
}
    