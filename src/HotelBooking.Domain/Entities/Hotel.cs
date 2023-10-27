using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Hotel : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Notes { get; set; }
    public string? WebsiteAddress { get; set; }
    public int ReviewCount { get; set; }
    public int ReviewRating { get; set; }
    public int CategoryId { get; set; }

    public virtual Address Address { get; set; } = default!;
    public virtual Category Category { get; set; } = default!;
    public virtual ICollection<HotelImage> HotelImages { get; set; } = new HashSet<HotelImage>();
    public virtual ICollection<HotelGroup> HotelGroups { get; set; } = new HashSet<HotelGroup>();
    public virtual ICollection<HotelFacilityHighlight> HotelFacilityHighlights { get; set; } = new HashSet<HotelFacilityHighlight>();
    public virtual ICollection<RoomType> RoomTypes { get; set; } = new HashSet<RoomType>();
}
