using HotelBooking.Application.Common.Mappings;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Application.DTOs.Payments;
public class CreateReservationDetailRequest : IMapFrom<ReservationDetail>
{
    public int RoomTypeId { get; set; }
    public int Quantity { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }

}
