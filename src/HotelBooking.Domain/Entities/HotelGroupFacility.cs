using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class HotelGroupFacility : BaseEntity
{

    public bool Available { get; set; } = true;
    public int FacilityId { get; set; }
    public int HotelGroupId { get; set; }
    public virtual Facility Facility { get; set; } = null!;
    public virtual HotelGroup HotelGroup { get; set; } = null!;
}
