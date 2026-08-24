using FluentValidation;
using Pustok.Application.Dtos.ProductDtos;

namespace Pustok.Application.Validators.ProductValidators;

public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
{
    public ProductUpdateDtoValidator()
    {
        RuleFor(x => x.Name).Must(x => x == null || !string.IsNullOrWhiteSpace(x))
    .WithMessage("Name cannot be empty or whitespace")
    .MaximumLength(256).WithMessage("max 256 chars");
        RuleFor(x => x.Description).Must(x => x == null || !string.IsNullOrWhiteSpace(x))
    .WithMessage("Description cannot be empty or whitespace")
    .MaximumLength(8192).WithMessage("max 8192 chars");
        RuleFor(x => x.ProductCode).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Code cannot be empty or whitespace").MaximumLength(32).WithMessage("max 32 chars");
        RuleFor(x => x.Price).GreaterThan(0m).WithMessage("Must be greater than 0");
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
        RuleFor(x => x.Discount).InclusiveBetween(0, 100).WithMessage("Discount can only be 0% to 100%");
        RuleFor(x => x.MainImage)
            .Must(x => x == null || x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x == null || x.ContentType.StartsWith("image/")).WithMessage("Must be image");
        RuleForEach(x => x.AdditionalImages).NotNull().WithMessage("Image is required")
            .Must(x => x == null || x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x == null || x.ContentType.StartsWith("image/")).WithMessage("Must be image");
        RuleFor(x => x.TagIds).Must(ids => ids == null || ids.Distinct().Count() == ids.Count).WithMessage("No duplicate IDs");
    }
}
