using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Payments;
public class PaymentResponse : BaseAuditableEntity, IMapFrom<Payment>
{
    public double Amount { get; set; }
    public long? TransactionNo { get; set; }
    public string? ResponseCode { get; set; }
    public string? TransactionStatus { get; set; }
    public string? SecureHash { get; set; }
    public string? BankCode { get; set; }
    public string? BankTranNo { get; set; }
    public string? CardType { get; set; }
    public string? PayDate { get; set; }
    public string IpAddress { get; set; } = default!;
    public string CreateDate { get; set; } = default!;
    public string? OrderInfo { get; set; }
    public string OrderType { get; set; } = default!;
    public int ReservationId { get; set; }
    public int TxnRef { get; set; }
}
