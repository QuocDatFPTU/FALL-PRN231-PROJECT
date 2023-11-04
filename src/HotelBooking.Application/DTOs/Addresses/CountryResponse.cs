using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Addresses;
public class CountryResponse : BaseEntity, IMapFrom<Country>
{
    public string Name { get; set; } = default!;
    public string Iso2 { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
