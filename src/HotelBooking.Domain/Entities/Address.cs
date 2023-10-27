using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Address : BaseEntity
{
    public int HotelId { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public int AreaId { get; set; }

    public virtual Hotel Hotel { get; set; } = default!;
    public virtual Area Area { get; set; } = default!;
    public virtual City City { get; set; } = default!;
    public virtual Country Country { get; set; } = default!;

}
