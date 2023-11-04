using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;
    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpPost("search")]
    public async Task<ActionResult<PaginatedResponse<HotelResponse>>> GetHotels(HotelSearchRequest request)
    {
        return Ok(await _hotelService.GetHotelsAsync(request));
    }
}
