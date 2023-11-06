using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.RoomTypes;
public class RoomTypeSoldOutResponse : BaseAuditableEntity, IMapFrom<RoomType>
{
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public string Description { get; set; } = default!;
    public int Availability { get; set; }
    public int ReviewCount { get; set; }
    public int ReviewRating { get; set; }

    public ICollection<RoomTypeImageResponse> RoomTypeImages { get; set; } = new HashSet<RoomTypeImageResponse>();
}
