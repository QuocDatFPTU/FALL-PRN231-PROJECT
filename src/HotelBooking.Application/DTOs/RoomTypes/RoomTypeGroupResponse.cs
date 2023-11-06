using AutoMapper;
using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.RoomTypes;
public class RoomTypeGroupResponse : IMapFrom<RoomTypeGroup>
{
    public int GroupId { get; set; }
    public string Name { get; set; } = default!;
    public int Order { get; set; }
    public ICollection<RoomTypeGroupFacilityResponse> RoomTypeGroupFacilities { get; set; } = new HashSet<RoomTypeGroupFacilityResponse>();

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RoomTypeGroup, RoomTypeGroupResponse>()
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Group.Name))
            .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Group.Order));
    }
}
