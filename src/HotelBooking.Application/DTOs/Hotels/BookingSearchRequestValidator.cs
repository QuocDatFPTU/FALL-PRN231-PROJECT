using FluentValidation;

namespace HotelBooking.Application.DTOs.Hotels;
public class BookingSearchRequestValidator : AbstractValidator<BookingSearchRequest>
{
    public BookingSearchRequestValidator()
    {
        RuleFor(x => x.CheckInDate)
            .LessThanOrEqualTo(x => x.CheckoutDate).WithMessage("CheckInDate must be less than CheckoutDate")
            .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("CheckInDate must be greater than today");

        RuleFor(x => x.CheckoutDate)
            .GreaterThanOrEqualTo(x => x.CheckInDate).WithMessage("CheckoutDate must be greater than CheckInDate")
            .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("CheckoutDate must be greater than today");
    }
}
