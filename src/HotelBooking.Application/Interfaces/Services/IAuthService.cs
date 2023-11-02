using HotelBooking.Application.DTOs.Auth;

namespace HotelBooking.Application.Interfaces.Services;
public interface IAuthService
{
    Task<AccessTokenResponse> SignInExternalAsync(ExternalAuthRequest externalAuthRequest);
}
