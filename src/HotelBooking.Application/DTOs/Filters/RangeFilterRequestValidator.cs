using FluentValidation;

namespace HotelBooking.Application.DTOs.Filters;
public class RangeFilterRequestValidator : AbstractValidator<RangeFilterRequest>
{
    public RangeFilterRequestValidator()
    {
        RuleFor(x => x.Ranges)
            .NotEmpty()
            .WithMessage("Ranges cannot be empty");

        RuleForEach(x => x.Ranges)
            .Must(x => x.From < x.To)
            .WithMessage("From must be less than to To");
    }
}
