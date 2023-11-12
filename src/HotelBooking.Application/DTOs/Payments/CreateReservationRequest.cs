using HotelBooking.Application.Common.Mappings;
using HotelBooking.Application.DTOs.Users;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Payments;

public class CreateReservationRequest : IMapFrom<Reservation>
{
    public double TotalPrice { get; set; }
    public CreateUserPaymentRequest Customer { get; set; } = default!;
    public ICollection<CreateReservationDetailRequest> RoomTypeRequests { get; set; } = default!;

    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<CreateReservationRequest, Reservation>()
            .ForMember(d => d.Guest, opt => opt.MapFrom(s => s.Customer))
            .ForMember(d => d.ReservationDetails, opt => opt.MapFrom(s => s.RoomTypeRequests));
    }

}