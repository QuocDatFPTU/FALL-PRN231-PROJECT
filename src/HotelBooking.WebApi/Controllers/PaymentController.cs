
using HotelBooking.Application.DTOs.Users;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IAuthService _authService;
    public PaymentController(IAuthService authService)
    {
        _authService = authService;
    }

    //[HttpPost("payment")]
    ////public async Task<ActionResult<UserResponse>> ProcessPayment()
    ////{
    ////    string ipAddress;
    ////    ipAddress = HttpContext.Request. ServerVariables["HTTP_X_FORWARDED_FOR"];
    ////}

}
