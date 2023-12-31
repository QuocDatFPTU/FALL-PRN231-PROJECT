﻿using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Payment : BaseAuditableEntity
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

    public int TxnRef { get; set; }
    public int ReservationId { get; set; }
    public virtual Reservation Reservation { get; set; } = default!;
}
