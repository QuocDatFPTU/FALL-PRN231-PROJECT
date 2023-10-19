using HotelBooking.Domain.Common.Interfaces;

namespace HotelBooking.Domain.Common;
public class BaseEntity : IEntity
{
    public int Id { get; set; }
}
