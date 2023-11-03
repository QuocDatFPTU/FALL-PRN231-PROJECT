namespace HotelBooking.Application.DTOs.Auth;
public sealed class ExternalAuthRequest
{
    public string IdToken { get; init; } = default!;
}
