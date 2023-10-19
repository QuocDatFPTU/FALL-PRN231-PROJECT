using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class ReservationDetail : BaseAuditableEntity
{
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int ReservationId { get; set; }
    public int RoomTypeId { get; set; }
    public virtual Reservation Reservation { get; set; } = default!;
    public virtual RoomType RoomType { get; set; } = default!;

}
