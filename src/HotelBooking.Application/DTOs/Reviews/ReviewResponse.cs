using AutoMapper;
using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Reviews;
public class ReviewResponse : IMapFrom<Review>
{
    public int ReviewId { get; set; }
    public string Title { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }
    public DateTimeOffset ReviewDate { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public ReviewerInfoResponse ReviewerInfo { get; set; } = default!;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Review, ReviewResponse>()
            .ForMember(d => d.ReviewId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.ReviewDate, opt => opt.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.CheckInDate, opt => opt.MapFrom(s => s.ReservationDetail.CheckInDate))
            .ForMember(d => d.CheckOutDate, opt => opt.MapFrom(s => s.ReservationDetail.CheckOutDate))
            .ForMember(d => d.ReviewerInfo, opt => opt.MapFrom(s => s.ReservationDetail));
    }
}
