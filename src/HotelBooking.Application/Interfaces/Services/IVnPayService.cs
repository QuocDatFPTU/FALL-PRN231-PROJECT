using HotelBooking.Application.DTOs.Payments;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Application.Interfaces.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}
