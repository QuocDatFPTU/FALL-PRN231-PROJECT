using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelImageResponse : BaseEntity, IMapFrom<HotelImage>
{
    public string ImageUrl { get; set; } = default!;
}
