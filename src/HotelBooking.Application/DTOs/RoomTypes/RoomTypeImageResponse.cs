using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.RoomTypes;
public class RoomTypeImageResponse : BaseEntity, IMapFrom<RoomTypeImage>
{
    public string ImageUrl { get; set; } = default!;
}
