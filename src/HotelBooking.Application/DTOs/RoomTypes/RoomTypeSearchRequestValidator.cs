using FluentValidation;

namespace HotelBooking.Application.DTOs.RoomTypes;
public class RoomTypeSearchRequestValidator : AbstractValidator<RoomTypeSearchRequest>
{
    public RoomTypeSearchRequestValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
