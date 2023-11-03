using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Application.Helpers;

namespace HotelBooking.Application.Interfaces.Services;
public interface ICityService
{
    Task<PaginatedList<CityResponse>> GetTopDestinationsAsync(int limit);
}
