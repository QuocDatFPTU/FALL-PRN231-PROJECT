using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class HotelFacilityHighlight : BaseEntity
{
    public bool Available { get; set; } = true;
    public int HotelId { get; set; }
    public int FacilityId { get; set; }
    public virtual Hotel Hotel { get; set; } = default!;
    public virtual Facility Facility { get; set; } = default!;

}
