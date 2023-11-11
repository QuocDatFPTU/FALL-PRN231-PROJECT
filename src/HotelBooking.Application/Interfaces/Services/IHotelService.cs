using HotelBooking.Application.DTOs.Hotels;
using HotelBooking.Application.DTOs.Reviews;
using HotelBooking.Application.DTOs.RoomTypes;
using HotelBooking.Application.Helpers;

namespace HotelBooking.Application.Interfaces.Services;
public interface IHotelService
{
    Task<PaginatedResponse<HotelResponse>> GetHotelsAsync(HotelSearchRequest hotelSearchRequest);
    Task<HotelDetailResponse> FindHotelAsync(RoomTypeSearchRequest roomTypeSearchRequest);
    Task<PaginatedResponse<ReviewResponse>> GetReviewsAsync(int hotelId, int pageIndex, int pageSize);
}
