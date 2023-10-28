using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Group : BaseEntity
{
    public string Name { get; set; } = default!;
    public int Order { get; set; }
    public virtual ICollection<RoomTypeGroup> RoomTypeGroups { get; set; } = new HashSet<RoomTypeGroup>();
    public virtual ICollection<HotelGroup> HotelGroups { get; set; } = new HashSet<HotelGroup>();
}
