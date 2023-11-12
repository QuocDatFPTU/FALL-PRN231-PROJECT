using HotelBooking.Application.Common.Mappings;
using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Areas;
public class RecommendationAreaResponse : BaseEntity, IMapFrom<Area>
{
    public string Name { get; set; } = default!;
    public int NoOfHotels { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CityId { get; set; }
    public CityResponse City { get; set; } = default!;
}
