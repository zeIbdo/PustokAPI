using FluentValidation;
using Pustok.Application.Dtos.ServiceDtos;

namespace Pustok.Application.Validators.ServiceValidators;

public class ServiceCreateDtoValidator : AbstractValidator<ServiceCreateDto>
{
    public ServiceCreateDtoValidator()
    {
        RuleFor(x => x.Title).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Title is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Description).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Image).Cascade(CascadeMode.Stop).NotNull().WithMessage("Image is required")
            .Must(x => x.Length <= 10 * 1024 * 1024).WithMessage("Cannot exceed 10 mb")
            .Must(x => x.ContentType.StartsWith("image/")).WithMessage("Must be image");
    }
}
