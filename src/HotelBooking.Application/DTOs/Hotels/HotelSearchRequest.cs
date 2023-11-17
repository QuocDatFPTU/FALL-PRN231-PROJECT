using HotelBooking.Application.DTOs.Bookings;
using HotelBooking.Application.DTOs.Filters;
using HotelBooking.Application.DTOs.Sorting;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelSearchRequest
{
    public int CityId { get; set; }
    public int Quantity { get; set; }
    public SortRequest Sorting { get; set; } = new SortRequest();
    public FilterRequest FilterRequest { get; set; } = new FilterRequest();
    public PageRequest Page { get; set; } = default!;
    public BookingSearchRequest SearchCriteria { get; set; } = default!;
}
