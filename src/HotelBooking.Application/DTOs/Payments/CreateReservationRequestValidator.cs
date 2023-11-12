using FluentValidation;

namespace HotelBooking.Application.DTOs.Payments;
public class CreateReservationRequestValidator : AbstractValidator<CreateReservationRequest>
{
    public CreateReservationRequestValidator()
    {
        RuleFor(x => x.Customer).NotNull();

        RuleFor(x => x.RoomTypeRequests).NotNull().NotEmpty();
    }
}
