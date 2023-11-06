using AutoMapper;
using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.RoomTypes;
public class RoomTypeGroupFacilityResponse : IMapFrom<RoomTypeGroupFacility>
{
    public int FacilityId { get; set; }
    public string Name { get; set; } = default!;
    public bool Available { get; set; } = true;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<RoomTypeGroupFacility, RoomTypeGroupFacilityResponse>()
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Facility.Name));
    }
}
