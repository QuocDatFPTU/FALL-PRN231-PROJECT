using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.Helpers;

namespace HotelBooking.Application.Interfaces.Services;
public interface IHotelService
{
    Task<PaginatedResponse<HotelResponse>> GetHotelsAsync(HotelSearchRequest hotelSearchRequest);
}
