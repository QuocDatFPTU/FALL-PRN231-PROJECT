using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Country : BaseEntity
{
    public string Name { get; set; } = default!;
    public string Iso2 { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
    public virtual ICollection<Address> Addresss { get; set; } = new HashSet<Address>();

}
