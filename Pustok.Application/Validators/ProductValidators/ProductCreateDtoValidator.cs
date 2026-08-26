using FluentValidation;
using Pustok.Application.Dtos.ProductDtos;

namespace Pustok.Application.Validators.ProductValidators;

public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
{
    public ProductCreateDtoValidator()
    {
        RuleFor(x => x.Name).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Name cannot be empty").MaximumLength(256).WithMessage("max 256 chars");
        RuleFor(x => x.Description).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be empty").MaximumLength(8192).WithMessage("max 8192 chars");
        RuleFor(x => x.ProductCode).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Code cannot be empty").MaximumLength(32).WithMessage("max 32 chars");
        RuleFor(x => x.Price).GreaterThan(0m).WithMessage("Must be greater than 0");
        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative");
        RuleFor(x => x.Discount).InclusiveBetween(0, 100).WithMessage("Discount can only be 0% to 100%");
        RuleFor(x => x.MainImage).Cascade(CascadeMode.Stop).NotNull().WithMessage("Main Image is required")
            .Must(x => x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x.ContentType.StartsWith("image/")).WithMessage("Must be image");
        RuleFor(x => x.AdditionalImages)
            .NotEmpty()
            .WithMessage("Additional Images are required(At least 1 image)");
        RuleForEach(x => x.AdditionalImages).Cascade(CascadeMode.Stop).NotNull().WithMessage("Additional Images are required")
            .Must(x => x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x.ContentType.StartsWith("image/")).WithMessage("Must be image");
        RuleFor(x => x.TagIds).Must(ids => ids == null || ids.Distinct().Count() == ids.Count).WithMessage("No duplicate IDs");
    }
}
