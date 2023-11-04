namespace HotelBooking.Application.DTOs.Hotels;
public class BookingSearchRequest
{
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckoutDate { get; set; }
}
