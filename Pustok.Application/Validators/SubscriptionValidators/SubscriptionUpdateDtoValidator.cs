using FluentValidation;
using Pustok.Application.Dtos.SubscriptionDtos;

namespace Pustok.Application.Validators.SubscriptionValidators;

public class SubscriptionUpdateDtoValidator : AbstractValidator<SubscriptionUpdateDto>
{
    public SubscriptionUpdateDtoValidator()
    {
        RuleFor(x => x.Email).Must(x => x == null || !string.IsNullOrWhiteSpace(x))
    .WithMessage("Email cannot be empty or whitespace")
            .MaximumLength(256).WithMessage("Cannot exceed 256 chars");
    }
}
