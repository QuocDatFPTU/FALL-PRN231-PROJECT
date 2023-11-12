using HotelBooking.Application.DTOs.Payments;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HotelBooking.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("return")]
    public async Task<IActionResult> CreatePaymentUrl(CreateReservationRequest request, string returnUrl)
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier)?.ConvertToInteger();
        var reservationResponse = await _paymentService.AddReservationAsync(id, request);
        var url = await _paymentService.CreatePaymentUrlAsync(reservationResponse, returnUrl);
        return Redirect(url);
        //return Ok(url);
    }

    [HttpGet("IPN")]
    public async Task<ActionResult<ReservationResponse>> PaymentCallback()
    {
        return await _paymentService.PaymentCallbackAsync(Request.Query);
    }
}
