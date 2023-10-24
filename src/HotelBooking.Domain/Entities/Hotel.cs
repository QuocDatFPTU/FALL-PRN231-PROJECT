using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Hotel : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string State { get; set; } = default!;
    public string Zip { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? Notes { get; set; }
    public string? Rating { get; set; }
    public string? WebsiteAddress { get; set; }
    public virtual ICollection<RoomType> RoomTypes { get; set; } = new HashSet<RoomType>();
}
