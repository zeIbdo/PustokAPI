using FluentValidation;
using Pustok.Application.Dtos.SubscriptionDtos;

namespace Pustok.Application.Validators.SubscriptionValidators;

public class SubscriptionCreateDtoValidator : AbstractValidator<SubscriptionCreateDto>
{
    public SubscriptionCreateDtoValidator()
    {
        RuleFor(x=>x.Email).Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Email is required")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
    }
}
