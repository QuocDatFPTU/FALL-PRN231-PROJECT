using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Areas;
public class AreaResponse : BaseEntity, IMapFrom<Area>
{
    public string Name { get; set; } = default!;
    public int NoOfHotels { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
