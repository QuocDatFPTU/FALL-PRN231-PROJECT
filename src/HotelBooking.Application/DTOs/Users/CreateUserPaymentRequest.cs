using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Users;
public class CreateUserPaymentRequest : IMapFrom<Guest>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}
