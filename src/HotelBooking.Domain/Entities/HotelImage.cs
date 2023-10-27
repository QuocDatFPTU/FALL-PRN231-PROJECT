using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class HotelImage : BaseEntity
{
    public string ImageUrl { get; set; } = default!;
    public int HotelId { get; set; }
    public virtual Hotel Hotel { get; set; } = default!;
}
