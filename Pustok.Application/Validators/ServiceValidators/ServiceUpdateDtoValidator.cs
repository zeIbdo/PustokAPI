using FluentValidation;
using Pustok.Application.Dtos.ServiceDtos;

namespace Pustok.Application.Validators.ServiceValidators;

public class ServiceUpdateDtoValidator : AbstractValidator<ServiceUpdateDto>
{
    public ServiceUpdateDtoValidator()
    {
        RuleFor(x => x.Title).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Title cannot be empty or whitespace").MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Description).Must(x => x == null || !string.IsNullOrWhiteSpace(x)).WithMessage("Description cannot be empty or whitespace").MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Image).Must(x => x == null || x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x == null || x.ContentType.StartsWith("image/")).WithMessage("Must be image");
    }
}
