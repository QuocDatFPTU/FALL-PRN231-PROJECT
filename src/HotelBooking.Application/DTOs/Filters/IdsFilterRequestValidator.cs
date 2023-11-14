using FluentValidation;

namespace HotelBooking.Application.DTOs.Filters;
public class IdsFilterRequestValidator : AbstractValidator<IdsFilterRequest>
{
    public IdsFilterRequestValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("Ids cannot be empty")
            .ForEach(_ => _.GreaterThan(0));
    }
}
