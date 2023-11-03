using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Users;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserResponse>> FindUser()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     .OrElseThrow(() => new ForbiddenAccessException())
                     .ConvertToInteger();
        return Ok(await _userService.FindUserAsync(id));
    }
}
