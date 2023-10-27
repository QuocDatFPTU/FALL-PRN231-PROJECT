using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class HotelGroup : BaseEntity
{
    public int GroupId { get; set; }
    public int HotelId { get; set; }
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual Group Group { get; set; } = null!;
    public virtual ICollection<HotelGroupFacility> HotelGroupFacilities { get; set; } = new List<HotelGroupFacility>();
}
