using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Reservation : BaseAuditableEntity
{
    public int TotalQuantity { get; set; }
    public double TotalCost { get; set; }
    public int GuestId { get; set; }
    public virtual Guest Guest { get; set; } = default!;
    public virtual Payment Payment { get; set; } = default!;
    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new HashSet<ReservationDetail>();

}
