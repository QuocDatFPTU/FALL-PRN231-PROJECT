using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Payment : BaseAuditableEntity
{
    public double Amount { get; set; }
    public string? Method { get; set; }
    public int ReservationId { get; set; }
    public virtual Reservation Reservation { get; set; } = default!;
}
