using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Payments;
public class ReservationDetailResponse : BaseAuditableEntity, IMapFrom<ReservationDetail>
{
    public int Quantity { get; set; }
    public double Price { get; set; }
    public bool IsReviewed { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    public int RoomTypeId { get; set; }
}
