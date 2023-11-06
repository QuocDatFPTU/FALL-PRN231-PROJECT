using AutoMapper;
using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelGroupFacilityResponse : IMapFrom<HotelGroupFacility>
{

    public int FacilityId { get; set; }
    public string Name { get; set; } = default!;
    public bool Available { get; set; } = true;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<HotelGroupFacility, HotelGroupFacilityResponse>()
            .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Facility.Name));
    }
}
