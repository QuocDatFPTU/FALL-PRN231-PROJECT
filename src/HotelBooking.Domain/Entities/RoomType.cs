using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class RoomType : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string RoomTypeStatus { get; set; } = default!;
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int HotelId { get; set; }
    public virtual Hotel Hotel { get; set; } = default!;

    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new HashSet<ReservationDetail>();
}
