using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class RoomTypeGroupFacility : BaseEntity
{

    public bool Available { get; set; } = true;
    public int FacilityId { get; set; }
    public int RoomTypeGroupId { get; set; }
    public virtual Facility Facility { get; set; } = default!;
    public virtual RoomTypeGroup RoomTypeGroup { get; set; } = default!;
}
