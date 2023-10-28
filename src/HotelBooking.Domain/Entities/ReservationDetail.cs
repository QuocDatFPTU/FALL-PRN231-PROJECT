using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class ReservationDetail : BaseAuditableEntity
{
    public int Quantity { get; set; }
    public double Price { get; set; }
    public bool IsReviewed { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int ReservationId { get; set; }
    public int RoomTypeId { get; set; }
    public virtual Reservation Reservation { get; set; } = default!;
    public virtual RoomType RoomType { get; set; } = default!;
    public virtual Review Review { get; set; } = default!;

}
