using HotelBooking.Application.DTOs.Bookings;

namespace HotelBooking.Application.DTOs.RoomTypes;
public class RoomTypeSearchRequest
{
    public int HotelId { get; set; }
    public int Quantity { get; set; }
    public BookingSearchRequest SearchCriteria { get; set; } = default!;
}
