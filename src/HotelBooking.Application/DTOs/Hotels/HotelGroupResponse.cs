using AutoMapper;
using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Hotels;
public class HotelGroupResponse : IMapFrom<HotelGroup>
{
    public int GroupId { get; set; }
    public string Name { get; set; } = default!;
    public int Order { get; set; }
    public ICollection<HotelGroupFacilityResponse> HotelGroupFacilities { get; set; } = new List<HotelGroupFacilityResponse>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<HotelGroup, HotelGroupResponse>()
            .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Group.Order))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Group.Name));
    }
}
