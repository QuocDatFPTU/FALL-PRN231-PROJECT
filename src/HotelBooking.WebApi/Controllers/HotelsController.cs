using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.DTOs.RoomTypes;
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
    public async Task<ActionResult<HotelSearchFilterResponse>> GetHotels(HotelSearchRequest request)
    {
        return Ok(new HotelSearchFilterResponse
        {
            Data = await _hotelService.GetHotelsAsync(request)
        });
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<HotelDetailResponse>> FindHotel(int id, RoomTypeSearchRequest request)
    {
        if (id != request.HotelId) throw new BadRequestException("Hotel id in request body and id url should be the same.");
        return Ok(await _hotelService.FindHotelAsync(request));
    }
}
