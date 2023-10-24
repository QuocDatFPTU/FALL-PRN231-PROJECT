using HotelBooking.Domain.Common.Interfaces;

namespace HotelBooking.Domain.Common;
public class BaseAuditableEntity : BaseEntity, IAuditableEntity, IEntity
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}
