using HotelBooking.Application.Common.Mappings;
using HotelBooking.Application.DTOs.Areas;
using HotelBooking.Application.DTOs.Cities;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Addresses;
public class AddressResponse : IMapFrom<Address>
{
    public AreaResponse Area { get; set; } = default!;
    public CityResponse City { get; set; } = default!;
    public CountryResponse Country { get; set; } = default!;
}
