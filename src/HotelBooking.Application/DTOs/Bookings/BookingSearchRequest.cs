﻿namespace HotelBooking.Application.DTOs.Bookings;
public class BookingSearchRequest
{
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckoutDate { get; set; }
}
