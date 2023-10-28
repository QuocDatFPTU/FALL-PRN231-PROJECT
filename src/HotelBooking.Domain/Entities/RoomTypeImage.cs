using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class RoomTypeImage : BaseEntity
{
    public string ImageUrl { get; set; } = default!;
    public int RoomTypeId { get; set; }
    public virtual RoomType RoomType { get; set; } = default!;
}
