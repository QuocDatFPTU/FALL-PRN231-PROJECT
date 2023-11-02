using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Guest : BaseAuditableEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AvatarUrl { get; set; }
    public string? ProviderKey { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
}
