using AutoMapper;
using HotelBooking.Application.Common.Exceptions;
using HotelBooking.Application.DTOs.Payments;
using HotelBooking.Application.Helpers;
using HotelBooking.Application.Interfaces.Repositories;
using HotelBooking.Application.Interfaces.Services;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace HotelBooking.Infrastructure.Services;
public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    public PaymentService(
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IEmailSender emailSender)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailSender = emailSender;
    }

    public async Task<ReservationResponse> AddReservationAsync(int? userId, CreateReservationRequest request)
    {
        var timeZoneId = _configuration["TimeZoneId"]!;
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);

        var reservation = _mapper.Map<Reservation>(request);

        if (userId.HasValue)
        {
            var user = (await _unitOfWork.Repository<Guest>().FindByAsync(_ => _.Id == userId.Value))
                        .OrElseThrow(() => new NotFoundException(nameof(Guest), userId.Value));
            _mapper.Map(request.Customer, user);
            reservation.Guest = user;
        }

        ModelStateDictionary modelState = new ModelStateDictionary();

        var roomTypes = await _unitOfWork.Repository<RoomType>()
            .FindAsync(expression: r => reservation.ReservationDetails.Select(_ => _.RoomTypeId).Contains(r.Id),
                      includeFunc: r => r.Include(_ => _.ReservationDetails).ThenInclude(_ => _.Reservation));

        foreach (var rd in reservation.ReservationDetails)
        {
            var checkInDate = rd.CheckInDate;
            var checkOutDate = rd.CheckOutDate;

            var roomType = roomTypes.Where(
                 r => r.Id == rd.RoomTypeId &&
                      r.ReservationDetails.Where(rd =>
                          rd.Reservation.Status != ReservationStatus.Canceled &&
                         (checkInDate >= rd.CheckInDate && checkInDate <= rd.CheckOutDate ||
                          checkOutDate >= rd.CheckInDate && checkOutDate <= rd.CheckOutDate ||
                          rd.CheckInDate >= checkInDate && rd.CheckInDate <= checkOutDate ||
                          rd.CheckOutDate >= checkInDate && rd.CheckOutDate <= checkOutDate))
                      .Sum(rd => rd.Quantity) + rd.Quantity <= r.Availability)
                      .FirstOrDefault();

            if (roomType == null)
            {
                modelState.AddModelError("roomTypes",
                    $"Roomtype id {rd.RoomTypeId} is not available" +
                    $" between the {checkInDate} - {checkOutDate}, " +
                    $"or not meet the required quantity of {rd.Quantity} within this time period.");

                continue;
            }

            rd.Price = roomType.Price;
        }

        if (!modelState.IsValid) throw new ValidationBadRequestException(modelState);

        var totalAmount = reservation.ReservationDetails.Sum(_ => _.Price * _.Quantity);
        if (totalAmount != request.TotalPrice)
        {
            throw new BadRequestException($"TotalPrice: {request.TotalPrice} not match `{totalAmount}`");
        }

        var totalQuantity = reservation.ReservationDetails.Sum(_ => _.Quantity);
        reservation.TotalAmount = totalAmount;
        reservation.TotalQuantity = totalQuantity;
        reservation.Status = ReservationStatus.Pending;
        reservation.Payment = new()
        {
            Amount = totalAmount,
            CreateDate = timeNow.ToString("yyyyMMddHHmmss"),
            IpAddress = UtilitiesExtensions.GetIpAddress(),
            OrderType = "170000"
        };

        await _unitOfWork.Repository<Reservation>().CreateAsync(reservation);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ReservationResponse>(reservation);
    }

    public Task<string> CreatePaymentUrlAsync(ReservationResponse request, string returnUrl)
    {
        var baseUrl = _configuration["Vnpay:BaseUrl"]!;
        var hashSecret = _configuration["Vnpay:HashSecret"]!;
        var version = _configuration["Vnpay:Version"]!;
        var command = _configuration["Vnpay:Command"]!;
        var tmnCode = _configuration["Vnpay:TmnCode"]!;
        var currCode = _configuration["Vnpay:CurrCode"]!;
        var locale = _configuration["Vnpay:Locale"]!;

        var vnpay = new VnPayLibrary();

        vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);

        vnpay.AddRequestData("vnp_Version", version);
        vnpay.AddRequestData("vnp_Command", command);
        vnpay.AddRequestData("vnp_TmnCode", tmnCode);
        vnpay.AddRequestData("vnp_CurrCode", currCode);
        vnpay.AddRequestData("vnp_Locale", locale);

        vnpay.AddRequestData("vnp_Amount", ((int)request.TotalAmount * 100).ToString());
        vnpay.AddRequestData("vnp_CreateDate", request.Payment.CreateDate);
        vnpay.AddRequestData("vnp_IpAddr", request.Payment.IpAddress);
        vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang: {request.Id}");
        vnpay.AddRequestData("vnp_OrderType", request.Payment.OrderType);
        vnpay.AddRequestData("vnp_TxnRef", request.Id.ToString());

        var paymentUrl = vnpay.CreateRequestUrl(baseUrl, hashSecret);
        return Task.FromResult(paymentUrl);
    }

    public async Task<ReservationResponse> PaymentCallbackAsync(IQueryCollection collections)
    {
        if (collections.IsNullOrEmpty())
        {
            throw new BadRequestException("Input data required!.");
        }

        var vnpay = new VnPayLibrary();

        foreach (var (key, value) in collections)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(key, value!);
            }
        }

        string vnp_HashSecret = _configuration["Vnpay:hashSecret"]!;

        int vnp_TxnRef = Convert.ToInt32(vnpay.GetResponseData("vnp_TxnRef"));
        double vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
        long vnp_TransactionNo = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
        string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
        string vnp_SecureHash = collections.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value!;
        string vnp_BankCode = vnpay.GetResponseData("vnp_BankCode");
        string vnp_BankTranNo = vnpay.GetResponseData("vnp_BankTranNo");
        string vnp_CardType = vnpay.GetResponseData("vnp_CardType");
        string vnp_PayDate = vnpay.GetResponseData("vnp_PayDate");
        string orderInfo = vnpay.GetResponseData("vnp_OrderInfo");

        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
        if (!checkSignature)
        {
            throw new BadRequestException("Invalid signature!.");
        }

        var reservation = (await _unitOfWork.Repository<Reservation>()
            .FindByAsync(_ => _.Id == vnp_TxnRef,
                         _ => _.Include(_ => _.Guest)
                               .Include(_ => _.Payment)
                               .Include(_ => _.ReservationDetails).ThenInclude(_ => _.RoomType.Hotel)))
            .OrElseThrow(() => new NotFoundException(nameof(Reservation), vnp_TxnRef));

        if (vnp_Amount != reservation.TotalAmount)
        {
            throw new BadRequestException("Invalid amount!.");
        }

        if (reservation.Status != ReservationStatus.Pending)
        {
            throw new BadRequestException("Reservation already confirmed!.");
        }

        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
        {
            reservation.Status = ReservationStatus.Confirmed;
        }
        else
        {
            reservation.Status = ReservationStatus.Canceled;
        }

        reservation.Payment.TransactionNo = vnp_TransactionNo;
        reservation.Payment.ResponseCode = vnp_ResponseCode;
        reservation.Payment.TransactionStatus = vnp_TransactionStatus;
        reservation.Payment.SecureHash = vnp_SecureHash;
        reservation.Payment.BankCode = vnp_BankCode;
        reservation.Payment.BankTranNo = vnp_BankTranNo;
        reservation.Payment.CardType = vnp_CardType;
        reservation.Payment.PayDate = vnp_PayDate;
        reservation.Payment.TxnRef = vnp_TxnRef;
        reservation.Payment.OrderInfo = orderInfo;

        await _unitOfWork.CommitAsync();

        var contend = await File.ReadAllTextAsync("wwwroot/Templates/Email/ReservationConfirmation.html");
        var contendRoom = reservation.ReservationDetails.Select(_ => $"<li>{_.Quantity} x {_.RoomType.Name} x {_.Price.ToString("N0", CultureInfo.GetCultureInfo("vi-VN"))} đ - {_.RoomType.Hotel.Name}</li>");
        _ = _emailSender.SendEmailAsync(
                reservation.Guest.Email!,
                $"Your Hotel Bookings Confirmation",
                string.Format(contend,
                   $"{reservation.Guest.FirstName} {reservation.Guest.LastName}",
                   reservation.Payment.TransactionNo,
                   reservation.TotalAmount.ToString("N0", CultureInfo.GetCultureInfo("vi-VN")),
                   reservation.Status,
                   reservation.CreatedAt,
                   string.Join(" ", contendRoom)));

        return _mapper.Map<ReservationResponse>(reservation);
    }
}
