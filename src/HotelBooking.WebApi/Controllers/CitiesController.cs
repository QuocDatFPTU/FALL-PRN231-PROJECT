using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;
    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    /// <param name="limit">default 10</param>
    [HttpGet("top-destinations")]
    public async Task<ActionResult<PaginatedList<CityResponse>>> GetTopDestinations(int limit)
    {
        return Ok(await _cityService.GetTopDestinationsAsync(limit));
    }
}
