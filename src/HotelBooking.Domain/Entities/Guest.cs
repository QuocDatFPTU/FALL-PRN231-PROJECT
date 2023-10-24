using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Guest : BaseAuditableEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string ZipCode { get; set; } = default!;

    public virtual ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();

}
