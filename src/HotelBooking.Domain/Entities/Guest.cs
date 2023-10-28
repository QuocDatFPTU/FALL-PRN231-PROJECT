using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Guest : BaseAuditableEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
}
