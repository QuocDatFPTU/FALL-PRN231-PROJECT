using HotelBooking.Application.DTOs.Auth;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthsController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("oauth2")]
    public async Task<ActionResult<AccessTokenResponse>> SignInExternalAsync(ExternalAuthRequest externalAuthRequest)
    {
        return Ok(await _authService.SignInExternalAsync(externalAuthRequest));
    }

}
