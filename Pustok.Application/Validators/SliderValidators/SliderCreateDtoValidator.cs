using FluentValidation;
using Pustok.Application.Dtos.SliderDtos;

namespace Pustok.Application.Validators.SliderValidators;

public class SliderCreateDtoValidator : AbstractValidator<SliderCreateDto>
{
    public SliderCreateDtoValidator()
    {
        RuleFor(x => x.Title).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Title is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Description).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Must be greater than 0");
        RuleFor(x => x.Image).Cascade(CascadeMode.Stop).NotNull().WithMessage("Image is required")
            .Must(x=>x.Length<=10*1024*1024).WithMessage("Cannot exceed 10 mb")
            .Must(x=>x.ContentType.StartsWith("image/")).WithMessage("Must be image");
    }
}
