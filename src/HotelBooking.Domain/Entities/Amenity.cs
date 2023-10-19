using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Amenity : BaseAuditableEntity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
