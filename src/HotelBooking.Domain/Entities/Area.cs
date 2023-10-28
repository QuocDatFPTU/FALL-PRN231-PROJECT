using HotelBooking.Domain.Common;

namespace HotelBooking.Domain.Entities;
public class Area : BaseEntity
{
    public string Name { get; set; } = default!;
    public int NoOfHotels { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public int CityId { get; set; }
    public virtual City City { get; set; } = default!;
    public virtual ICollection<Address> Addresss { get; set; } = new HashSet<Address>();
}
