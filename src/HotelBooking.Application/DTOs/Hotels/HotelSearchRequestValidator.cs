using FluentValidation;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelSearchRequestValidator : AbstractValidator<HotelSearchRequest>
{
    public HotelSearchRequestValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
