using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Facility : BaseEntity
{
    public string Name { get; set; } = default!;

    public virtual ICollection<HotelGroupFacility> HotelGroupFacilities { get; set; } = new HashSet<HotelGroupFacility>();
    public virtual ICollection<RoomTypeGroupFacility> RoomTypeGroupFacilities { get; set; } = new HashSet<RoomTypeGroupFacility>();

    public virtual ICollection<HotelFacilityHighlight> HotelFacilityHighlights { get; set; } = new HashSet<HotelFacilityHighlight>();
}
