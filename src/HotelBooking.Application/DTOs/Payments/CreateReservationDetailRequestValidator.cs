using FluentValidation;

namespace HotelBooking.Application.DTOs.Payments;
public class CreateReservationDetailRequestValidator : AbstractValidator<CreateReservationDetailRequest>
{
    public CreateReservationDetailRequestValidator()
    {
        RuleFor(x => x.Quantity).GreaterThan(0);

        RuleFor(x => x.CheckInDate)
            .LessThanOrEqualTo(x => x.CheckOutDate).WithMessage("CheckInDate must be less than CheckOutDate")
            .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("CheckInDate must be greater than today");

        RuleFor(x => x.CheckOutDate)
            .GreaterThanOrEqualTo(x => x.CheckInDate).WithMessage("CheckOutDate must be greater than CheckInDate")
            .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("CheckOutDate must be greater than today");
    }
}
