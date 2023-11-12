using HotelBooking.Domain.Common;
using HotelBooking.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities;
public class Reservation : BaseAuditableEntity
{
    public int TotalQuantity { get; set; }
    public double TotalAmount { get; set; }

    [Column(TypeName = "nvarchar(24)")]
    public ReservationStatus Status { get; set; }
    public int GuestId { get; set; }
    public virtual Guest Guest { get; set; } = default!;
    public virtual Payment Payment { get; set; } = default!;
    public virtual ICollection<ReservationDetail> ReservationDetails { get; set; } = new HashSet<ReservationDetail>();

}
