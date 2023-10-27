using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Review : BaseAuditableEntity
{
    public string Title { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }
    public int ReservationDetailId { get; set; }
    public virtual ReservationDetail ReservationDetail { get; set; } = default!;
}
