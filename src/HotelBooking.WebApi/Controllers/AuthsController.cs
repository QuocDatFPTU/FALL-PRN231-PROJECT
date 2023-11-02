using AutoMapper;
using HotelBooking.Application.DTOs.Auth;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public AuthsController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAuthService authService)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("oauth2")]
    public async Task<IActionResult> SignInExternalAsync(ExternalAuthRequest externalAuthRequest)
    {
        var response = await _authService.SignInExternalAsync(externalAuthRequest);
        return Ok(response);
    }

}
