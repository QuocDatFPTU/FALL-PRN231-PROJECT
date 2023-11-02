using HotelBooking.Application.DTOs.Auth;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace HotelBooking.Infrastructure.Services;
public class AuthService : IAuthService
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly TokenHelper<Guest> _tokenHelper;
    public AuthService(
        IUnitOfWork unitOfWork,
        TokenHelper<Guest> tokenHelper)
    {
        _unitOfWork = unitOfWork;
        _tokenHelper = tokenHelper;
    }

    public async Task<AccessTokenResponse> SignInExternalAsync(ExternalAuthRequest externalAuthRequest)
    {
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(externalAuthRequest.IdToken);
        var subject = jwtSecurityToken.Subject.OrElseThrow(() => new UnauthorizedAccessException("Subject in payload is null"));

        var claims = jwtSecurityToken.Claims;
        var email = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
        var name = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
        var picture = claims.FirstOrDefault(x => x.Type == "picture")?.Value;

        if (await _unitOfWork.Repository<Guest>().FindByAsync(x => x.ProviderKey == subject) is not { } user)
        {
            user = new Guest()
            {
                ProviderKey = subject,
                Email = email,
                FirstName = name,
                AvatarUrl = picture,
            };

            await _unitOfWork.Repository<Guest>().CreateAsync(user);
            await _unitOfWork.CommitAsync();
        }
        return await _tokenHelper.CreateTokenAsync(user);
    }
}

