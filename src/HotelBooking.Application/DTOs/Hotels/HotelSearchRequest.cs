using HotelBooking.Application.DTOs.Bookings;
using HotelBooking.Application.DTOs.Filters;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelSearchRequest
{
    public int CityId { get; set; }
    public int Quantity { get; set; }
    public PageRequest Page { get; set; } = default!;
    public BookingSearchRequest SearchCriteria { get; set; } = default!;
}
