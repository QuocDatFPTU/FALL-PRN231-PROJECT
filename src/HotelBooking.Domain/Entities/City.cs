using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class City : BaseEntity
{
    public string Name { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ImageUrl { get; set; } = default!;
    public int NoOfHotels { get; set; }
    public int CountryId { get; set; }
    public virtual Country Country { get; set; } = default!;
    public virtual ICollection<Address> Addresss { get; set; } = new HashSet<Address>();
    public virtual ICollection<Area> Areas { get; set; } = new HashSet<Area>();

}
