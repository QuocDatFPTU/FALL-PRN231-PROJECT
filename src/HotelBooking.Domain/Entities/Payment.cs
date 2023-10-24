using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Payment : BaseAuditableEntity
{
    public decimal Amount { get; set; }
    public string? Notes { get; set; }
    public string? PaymentMethod { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int ReservationId { get; set; }
    public virtual Reservation Reservation { get; set; } = default!;
}
