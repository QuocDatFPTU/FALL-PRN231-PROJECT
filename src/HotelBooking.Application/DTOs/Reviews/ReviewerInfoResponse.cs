using AutoMapper;
using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Reviews;
public class ReviewerInfoResponse : IMapFrom<ReservationDetail>
{
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ReservationDetail, ReviewerInfoResponse>()
            .ForMember(d => d.RoomTypeName, opt => opt.MapFrom(s => s.RoomType.Name))
            .ForMember(d => d.FirstName, opt => opt.MapFrom(s => s.Reservation.Guest.FirstName))
            .ForMember(d => d.LastName, opt => opt.MapFrom(s => s.Reservation.Guest.LastName))
            .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Reservation.Guest.Email))
            .ForMember(d => d.PhoneNumber, opt => opt.MapFrom(s => s.Reservation.Guest.PhoneNumber))
            .ForMember(d => d.AvatarUrl, opt => opt.MapFrom(s => s.Reservation.Guest.AvatarUrl));
    }

}
