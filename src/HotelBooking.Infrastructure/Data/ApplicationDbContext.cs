using HotelBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelFacilityHighlight> HotelFacilityHighlights { get; set; }
        public virtual DbSet<HotelGroup> HotelGroups { get; set; }
        public virtual DbSet<HotelGroupFacility> HotelGroupFacilities { get; set; }
        public virtual DbSet<HotelImage> HotelImages { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationDetail> ReservationDetails { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<RoomTypeGroup> RoomTypeGroups { get; set; }
        public virtual DbSet<RoomTypeGroupFacility> RoomTypeGroupFacilities { get; set; }
        public virtual DbSet<RoomTypeImage> RoomTypeImages { get; set; }
    }
}
