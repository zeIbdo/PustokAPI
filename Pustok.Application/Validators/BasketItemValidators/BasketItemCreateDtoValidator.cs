using FluentValidation;
using Pustok.Application.Dtos.BasketItemDtos;

namespace Pustok.Application.Validators.BasketItemValidators;

public class BasketItemCreateDtoValidator : AbstractValidator<BasketItemCreateDto>
{
    public BasketItemCreateDtoValidator()
    {
        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be greater than 0");
    }
}
