using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Reservation : BaseAuditableEntity
{

    public int NumberOfGuests { get; set; }
    public bool HasBreakfast { get; set; }
    public bool HasAllInclusive { get; set; }
    public bool HasPaid { get; set; }
    public decimal TotalCost { get; set; }
    public string? Notes { get; set; }
    public int GuestId { get; set; }
    public virtual Guest Guest { get; set; } = default!;

    public virtual Payment Payment { get; set; } = default!;
    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new HashSet<ReservationDetail>();

}
