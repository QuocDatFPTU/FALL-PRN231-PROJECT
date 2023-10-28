using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class RoomType : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public string Description { get; set; } = default!;
    public int Availability { get; set; }
    public int ReviewCount { get; set; }
    public int ReviewRating { get; set; }
    public int HotelId { get; set; }
    public virtual Hotel Hotel { get; set; } = default!;
    public virtual ICollection<RoomTypeImage> RoomTypeImages { get; set; } = new HashSet<RoomTypeImage>();
    public virtual ICollection<RoomTypeGroup> RoomTypeGroups { get; set; } = new HashSet<RoomTypeGroup>();
    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new HashSet<ReservationDetail>();
}
