using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public virtual ICollection<Hotel> Hotels { get; set; } = new HashSet<Hotel>();
}
