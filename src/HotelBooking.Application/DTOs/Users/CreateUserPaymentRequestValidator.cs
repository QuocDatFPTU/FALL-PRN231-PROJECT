using FluentValidation;

namespace HotelBooking.Application.DTOs.Users;
public class CreateUserPaymentRequestValidator : AbstractValidator<CreateUserPaymentRequest>
{
    public CreateUserPaymentRequestValidator()
    {
        RuleFor(_ => _.Email).EmailAddress();
    }
}
