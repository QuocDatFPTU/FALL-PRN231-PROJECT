using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class RoomTypeGroup : BaseEntity
{
    public int GroupId { get; set; }
    public int RoomTypeId { get; set; }
    public virtual RoomType RoomType { get; set; } = default!;
    public virtual Group Group { get; set; } = default!;
    public virtual ICollection<RoomTypeGroupFacility> RoomTypeGroupFacilities { get; set; } = new HashSet<RoomTypeGroupFacility>();
}
