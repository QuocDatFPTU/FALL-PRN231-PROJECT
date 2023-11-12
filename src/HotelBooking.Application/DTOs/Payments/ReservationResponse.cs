using HotelBooking.Application.Common.Mappings;
using HotelBooking.Application.DTOs.Users;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Entities;
using HotelBooking.Domain.Enums;

namespace HotelBooking.Application.DTOs.Payments;

public class ReservationResponse : BaseAuditableEntity, IMapFrom<Reservation>
{
    public int TotalQuantity { get; set; }
    public double TotalAmount { get; set; }
    public ReservationStatus Status { get; set; }
    public int GuestId { get; set; }
    public UserResponse Guest { get; set; } = default!;
    public PaymentResponse Payment { get; set; } = default!;
    public ICollection<ReservationDetailResponse> ReservationDetails { get; set; } = new HashSet<ReservationDetailResponse>();

}