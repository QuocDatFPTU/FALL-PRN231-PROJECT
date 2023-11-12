using HotelBooking.Application.DTOs.Payments;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Application.Interfaces.Services;
public interface IPaymentService
{
    Task<ReservationResponse> AddReservationAsync(int? userId, CreateReservationRequest request);
    Task<string> CreatePaymentUrlAsync(ReservationResponse request, string returnUrl);
    Task<ReservationResponse> PaymentCallbackAsync(IQueryCollection collections);
}
