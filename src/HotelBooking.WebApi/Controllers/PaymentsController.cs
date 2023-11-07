using HotelBooking.Application.DTOs.Payments;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    public PaymentsController(IVnPayService vnPayService)
    {
        _vnPayService = vnPayService;
    }

    [HttpGet("vnpay-url")]
    public IActionResult CreatePaymentUrl([FromQuery] PaymentInformationModel model)
    {
        var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
        return Redirect(url);
    }

    [HttpGet("vnpay-callback")]
    public IActionResult PaymentCallback()
    {
        var response = _vnPayService.PaymentExecute(Request.Query);
        return Ok(response);
    }
}
